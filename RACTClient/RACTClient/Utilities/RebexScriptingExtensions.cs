using System;
using System.Text;
using Rebex.TerminalEmulation;
using RACTCommonClass;

namespace RACTClient.Utilities.Extensions
{
    public static class RebexScriptingExtensions
    {
        public static void ExecuteConnectionCommand(this Scripting scripting, FACT_DefaultConnectionCommandSet commandSet, DeviceInfo deviceInfo, int timeoutMs = 5000)
        {
            if (scripting == null || commandSet == null || commandSet.CommandList.Count == 0) return;

            try
            {
                // 1. RCCS 로그인 전처리 (+d 대기)
                if (AppGlobal.s_ConnectionMode == 1)
                {
                    // 화면에 이미 +d가 있는지 확인 (GetText 대체 로직 사용)
                    if (!IsPromptVisibleOnScreen(scripting, "+d"))
                    {
                        var targetEvent = ScriptEvent.FromString("+d");
                        scripting.WaitFor(targetEvent | ScriptEvent.Delay(1000));
                    }
                    scripting.Send("\r");
                }

                // 2. 첫 프롬프트 대기
                bool isTL1 = (deviceInfo.TerminalConnectInfo.TelnetPort == 1023);
                string firstPromptStr = isTL1 ? commandSet.CommandList[0].TL1_Prompt : commandSet.CommandList[0].Prompt;

                if (!string.IsNullOrEmpty(firstPromptStr))
                {
                    // [핵심] 화면 버퍼 확인 (GetCell 기반)
                    if (IsPromptVisibleOnScreen(scripting, firstPromptStr))
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, $"Prompt '{firstPromptStr}' detected on screen. Skipping wait.");
                    }
                    else
                    {
                        var promptEvent = GetPromptEvent(firstPromptStr);
                        var delayEvent = ScriptEvent.Delay(timeoutMs);

                        ScriptMatch match = scripting.WaitFor(promptEvent | delayEvent);

                        if (match.IsEventMatched(delayEvent))
                        {
                            // 타임아웃 시 마지막으로 화면 다시 확인
                            if (!IsPromptVisibleOnScreen(scripting, firstPromptStr))
                            {
                                throw new TimeoutException($"Timeout waiting for initial prompt '{firstPromptStr}'");
                            }
                        }
                    }
                }

                // 3. 명령어 순차 실행
                for (int i = 1; i < commandSet.CommandList.Count; i++)
                {
                    var cmdItem = commandSet.CommandList[i];
                    string cmdKey = isTL1 ? cmdItem.TL1_CMD : cmdItem.CMD;
                    string promptStr = isTL1 ? cmdItem.TL1_Prompt : cmdItem.Prompt;

                    string commandToSend = ReplaceMacros(cmdKey, deviceInfo);

                    scripting.SendCommand(commandToSend);

                    if (!string.IsNullOrEmpty(promptStr))
                    {
                        var promptEvent = GetPromptEvent(promptStr);
                        var delayEvent = ScriptEvent.Delay(timeoutMs);

                        ScriptMatch match = scripting.WaitFor(promptEvent | delayEvent);

                        if (match.IsEventMatched(delayEvent))
                        {
                            if (!IsPromptVisibleOnScreen(scripting, promptStr))
                            {
                                throw new TimeoutException($"Timeout waiting for prompt '{promptStr}' after command '{commandToSend}'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Rebex Script Execution Failed: {ex.Message}", ex);
            }
        }

        public static void ExecuteBatch(this Scripting scripting, string fullCommand, string expectedPromptStr, int timeoutMs = 10000)
        {
            if (scripting == null || string.IsNullOrWhiteSpace(fullCommand)) return;

            // [커스텀 DetectPrompt 적용 가능]
            // 만약 서버 특성상 DetectPrompt()가 실패한다면, 
            // 제공해주신 사용자 정의 DetectPrompt 로직을 이곳에 적용할 수 있습니다.
            if (string.IsNullOrEmpty(expectedPromptStr))
            {
                try { scripting.DetectPrompt(); } catch { }
            }

            string[] cmdLines = fullCommand.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string cmd in cmdLines)
            {
                if (string.IsNullOrWhiteSpace(cmd)) continue;

                scripting.SendCommand(cmd);

                ScriptEvent targetEvent;
                if (!string.IsNullOrEmpty(expectedPromptStr))
                {
                    targetEvent = ScriptEvent.FromString(expectedPromptStr);
                }
                else
                {
                    targetEvent = ScriptEvent.Prompt;
                }

                var delayEvent = ScriptEvent.Delay(timeoutMs);
                ScriptMatch match = scripting.WaitFor(targetEvent | delayEvent);

                if (match.IsEventMatched(delayEvent) && !match.IsPrompt)
                {
                    throw new TimeoutException($"Batch execution timed out waiting for prompt. Last command: {cmd}");
                }
            }
        }

        // ---------------------------------------------------------------------
        // Helper Methods
        // ---------------------------------------------------------------------

        private static string ReplaceMacros(string cmdKey, DeviceInfo deviceInfo)
        {
            if (string.IsNullOrEmpty(cmdKey)) return "";

            return cmdKey.Replace("${USERID1}", deviceInfo.TelnetID1)
                         .Replace("${PASSWORD1}", deviceInfo.TelnetPwd1)
                         .Replace("${USERID2}", deviceInfo.TelnetID2)
                         .Replace("${PASSWORD2}", deviceInfo.TelnetPwd2);
        }

        /// ---------------------------------------------------------------------
        // [수정] 화면 텍스트 추출 최적화 (전체 반복 -> 커서 라인만 확인)
        // ---------------------------------------------------------------------
        private static bool IsPromptVisibleOnScreen(Scripting scripting, string promptPattern)
        {
            try
            {
                var screen = scripting.Terminal.Screen;

                // [최적화] 전체 화면을 다 돌지 않고, 커서가 있는 라인(CursorTop)과 
                // 그 윗 라인(최대 2~3줄)만 텍스트로 변환하여 검사합니다.
                // 대부분의 프롬프트는 커서가 위치한 줄에 존재하기 때문입니다.
                string relevantText = GetTextAroundCursor(screen, 2);

                if (string.IsNullOrEmpty(relevantText)) return false;

                if (!promptPattern.Contains("|"))
                {
                    return relevantText.Contains(promptPattern);
                }

                string[] parts = promptPattern.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    if (relevantText.Contains(part)) return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 커서 위치를 기준으로 최근 n개의 라인 텍스트만 추출합니다.
        /// (GetCell 반복 횟수 최소화)
        /// </summary>
        private static string GetTextAroundCursor(TerminalScreen screen, int lookBackLines)
        {
            if (screen == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            // 현재 커서의 Y좌표 (행)
            int cursorRow = screen.CursorTop;

            // 시작 행 계산 (0보다 작아지지 않게 방어)
            int startRow = Math.Max(0, cursorRow - lookBackLines);
            int endRow = cursorRow;

            // 필요한 행(Row)만 반복
            for (int row = startRow; row <= endRow; row++)
            {
                // 해당 행의 모든 열(Col) 반복
                for (int col = 0; col < screen.Columns; col++)
                {
                    TerminalCell cell = screen.GetCell(col, row);

                    // [수정] TerminalCell은 struct이므로 null 체크 불가능
                    // 대신 문자가 '\0'(null char)인 경우 공백으로 처리
                    char c = cell.Character;
                    if (c == '\0')
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static ScriptEvent GetPromptEvent(string promptStr)
        {
            if (!promptStr.Contains("|"))
            {
                return ScriptEvent.FromString(promptStr);
            }

            // "n:|:" -> "n:" OR ":"
            string[] parts = promptStr.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return ScriptEvent.FromString(promptStr);

            ScriptEvent combinedEvent = ScriptEvent.FromString(parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                combinedEvent = combinedEvent | ScriptEvent.FromString(parts[i]);
            }

            return combinedEvent;
        }
    }
}