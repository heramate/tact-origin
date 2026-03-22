using RACTCommonClass;
using Rebex.TerminalEmulation;
using System;
using System.Text;

namespace RACTClient.Utilities.Extensions
{
    public static class RebexScriptingExtensions
    {
        public static void ExecuteConnectionCommand(
            this Scripting scripting,
            FACT_DefaultConnectionCommandSet commandSet,
            DeviceInfo deviceInfo,
            int timeoutMs = 5000)
        {
            if (scripting == null || commandSet == null || commandSet.CommandList.Count == 0)
                return;

            try
            {
                // 1. RCCS 로그인 전처리 (+d 대기)
                if (AppGlobal.s_ConnectionMode == 1)
                {
                    // 화면에 이미 +d가 보이면 추가 대기를 생략합니다.
                    if (!IsPromptVisibleNearCursor(scripting, "+d"))
                    {
                        var targetEvent = ScriptEvent.FromString("+d");
                        scripting.WaitFor(targetEvent | ScriptEvent.Delay(1000));
                    }

                    scripting.Send("\r");
                }

                // 2. 첫 프롬프트 대기
                bool isTL1 = (deviceInfo.TerminalConnectInfo.TelnetPort == 1023);
                string firstPromptStr = isTL1
                    ? commandSet.CommandList[0].TL1_Prompt
                    : commandSet.CommandList[0].Prompt;

                if (!string.IsNullOrEmpty(firstPromptStr))
                {
                    if (IsPromptVisibleNearCursor(scripting, firstPromptStr))
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(
                            E_FileLogType.Infomation,
                            $"Prompt '{firstPromptStr}' detected on screen. Skipping wait.");
                    }
                    else
                    {
                        var promptEvent = BuildPromptEvent(firstPromptStr);
                        var delayEvent = ScriptEvent.Delay(timeoutMs);

                        ScriptMatch match = scripting.WaitFor(promptEvent | delayEvent);

                        if (match.IsEventMatched(delayEvent))
                        {
                            if (!IsPromptVisibleNearCursor(scripting, firstPromptStr))
                            {
                                throw new TimeoutException(
                                    $"Timeout waiting for initial prompt '{firstPromptStr}'");
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

                    string commandToSend = ExpandCommandMacros(cmdKey, deviceInfo);
                    string commandLabel = DescribeCommandKey(cmdKey);
                    bool isEmptyCommand = string.IsNullOrWhiteSpace(commandToSend);
                    string readyPrompt = DetectReadyPromptNearCursor(scripting);

                    AppGlobal.s_FileLogProcessor.PrintLog(
                        E_FileLogType.Infomation,
                        $"AutoLogin Step {i}: command={commandLabel}, empty={isEmptyCommand}, prompt='{promptStr}'");

                    if (isEmptyCommand && !string.IsNullOrEmpty(readyPrompt))
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(
                            E_FileLogType.Infomation,
                            $"Ready prompt '{readyPrompt}' already visible. Skipping empty command step for prompt '{promptStr}'.");
                        continue;
                    }

                    if (isEmptyCommand &&
                        !string.IsNullOrEmpty(promptStr) &&
                        ShouldSkipEmptyCommandStep(scripting, promptStr))
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(
                            E_FileLogType.Infomation,
                            $"Prompt '{promptStr}' already visible. Skipping empty command step.");
                        continue;
                    }

                    if (isEmptyCommand)
                    {
                        // Legacy script behavior sends only Enter when an ID/PW slot is blank.
                        scripting.Send("\r");
                        scripting.Process(200);
                    }
                    else
                    {
                        scripting.SendCommand(commandToSend);
                    }

                    if (!string.IsNullOrEmpty(promptStr))
                    {
                        var promptEvent = BuildPromptEvent(promptStr);
                        var delayEvent = ScriptEvent.Delay(timeoutMs);

                        ScriptMatch match = scripting.WaitFor(promptEvent | delayEvent);

                        if (match.IsEventMatched(delayEvent))
                        {
                            readyPrompt = DetectReadyPromptNearCursor(scripting);
                            if (isEmptyCommand && !string.IsNullOrEmpty(readyPrompt))
                            {
                                AppGlobal.s_FileLogProcessor.PrintLog(
                                    E_FileLogType.Infomation,
                                    $"Ready prompt '{readyPrompt}' detected after timeout. Accepting empty command step for prompt '{promptStr}'.");
                                continue;
                            }

                            if (!IsPromptVisibleNearCursor(scripting, promptStr))
                            {
                                string screenExcerpt = GetScreenExcerpt(scripting);
                                throw new TimeoutException(
                                    $"Timeout waiting for prompt '{promptStr}' after command '{commandToSend}'. Screen='{screenExcerpt}'");
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

        /// <summary>
        /// 여러 명령어(배치)를 실행합니다.
        /// </summary>
        /// <param name="scripting">Rebex Scripting 인스턴스</param>
        /// <param name="fullCommand">실행할 명령어 뭉치 (줄바꿈 구분)</param>
        /// <param name="expectedPromptStr">대기할 프롬프트(Optional)</param>
        /// <param name="logAction">로그 기록용 Action</param>
        /// <param name="intervalMs">명령어 간 실행 간격 (밀리초)</param>
        public static void ExecuteBatch(
            this Scripting scripting,
            string fullCommand,
            string expectedPromptStr,
            Action<string> logAction,
            int intervalMs = 1000)
        {
            if (scripting == null || string.IsNullOrWhiteSpace(fullCommand))
                return;

            // 1. 프롬프트 설정 (명시적 지정 또는 자동 감지)
            if (!string.IsNullOrEmpty(expectedPromptStr))
            {
                scripting.Prompt = expectedPromptStr;
            }
            else if (string.IsNullOrEmpty(scripting.Prompt))
            {
                scripting.DetectPrompt();
                logAction?.Invoke($"Detected Prompt: {scripting.Prompt}");
            }

            // paging(more)은 전역 AutoResponder가 처리하고,
            // 여기서는 프롬프트 복귀만 기준으로 다음 명령으로 진행합니다.
            scripting.Timeout = 30000;

            var cmdLines = fullCommand.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in cmdLines)
            {
                var cmd = raw?.Trim();
                if (string.IsNullOrEmpty(cmd)) continue;

                logAction?.Invoke($"👉 Sending Command: {cmd}");
                scripting.SendCommand(cmd);

                try
                {
                    scripting.WaitFor(ScriptEvent.Prompt);
                    logAction?.Invoke($"✔️ Command '{cmd}' executed successfully.");
                }
                catch (Exception ex)
                {
                    logAction?.Invoke($"❌ Timeout or Error during '{cmd}': {ex.Message}");

                    // Rebex 경합으로 WaitFor가 늦게 실패해도 화면에 프롬프트가 있으면 계속 진행합니다.
                    if (IsPromptVisibleNearCursor(scripting, scripting.Prompt))
                    {
                        logAction?.Invoke("✔️ Prompt detected on screen. Proceeding next command.");
                        continue;
                    }

                    throw;
                }

                if (intervalMs > 0)
                {
                    // Thread.Sleep 대신 Process를 사용해 대기 중 이벤트 처리를 유지합니다.
                    scripting.Process(intervalMs);
                }
            }
        }

        // ExecuteBatch_tmp는 제거되었고, 현재는 ExecuteBatch가 프롬프트/페이징 대응을 포함한 표준 구현입니다.

        // ---------------------------------------------------------------------
        // Private Helpers
        // ---------------------------------------------------------------------

        /// <summary>
        /// 연결 커맨드 템플릿의 사용자 ID/비밀번호 매크로를 실제 장비 값으로 치환합니다.
        /// </summary>
        private static string ExpandCommandMacros(string cmdKey, DeviceInfo deviceInfo)
        {
            if (string.IsNullOrEmpty(cmdKey))
                return string.Empty;

            return cmdKey.Replace("${USERID1}", deviceInfo.TelnetID1)
                         .Replace("${PASSWORD1}", deviceInfo.TelnetPwd1)
                         .Replace("${USERID2}", deviceInfo.TelnetID2)
                         .Replace("${PASSWORD2}", deviceInfo.TelnetPwd2);
        }

        private static string DescribeCommandKey(string cmdKey)
        {
            if (string.IsNullOrWhiteSpace(cmdKey))
                return "<empty>";

            string normalized = cmdKey.Trim();
            if (normalized.Equals("${USERID1}", StringComparison.OrdinalIgnoreCase)) return "USERID1";
            if (normalized.Equals("${PASSWORD1}", StringComparison.OrdinalIgnoreCase)) return "PASSWORD1";
            if (normalized.Equals("${USERID2}", StringComparison.OrdinalIgnoreCase)) return "USERID2";
            if (normalized.Equals("${PASSWORD2}", StringComparison.OrdinalIgnoreCase)) return "PASSWORD2";

            return normalized.Length > 30 ? normalized.Substring(0, 30) + "..." : normalized;
        }

        /// <summary>
        /// 프롬프트가 현재 화면의 커서 주변에 이미 표시되어 있는지 빠르게 확인합니다.
        /// 전체 화면을 스캔하지 않고 최근 몇 줄만 검사해 WaitFor 경합을 줄입니다.
        /// </summary>
        private static bool IsPromptVisibleNearCursor(Scripting scripting, string promptPattern)
        {
            try
            {
                var screen = scripting.Terminal.Screen;
                string relevantText = ReadTextAroundCursor(screen, 20);

                if (string.IsNullOrEmpty(relevantText))
                    return false;

                if (!promptPattern.Contains("|"))
                {
                    return relevantText.Contains(promptPattern);
                }

                string[] parts = promptPattern.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    if (relevantText.Contains(part))
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 빈 명령 단계는 "이미 다음 단계가 끝난 상태"일 때만 건너뜁니다.
        /// 예: d:|# 에서 현재 d: 가 보이는 것은 "입력 대기"이므로 skip 하면 안 되고,
        /// 현재 # 가 보일 때만 skip 해야 합니다.
        /// </summary>
        private static bool ShouldSkipEmptyCommandStep(Scripting scripting, string promptPattern)
        {
            if (string.IsNullOrWhiteSpace(promptPattern))
                return false;

            if (!promptPattern.Contains("|"))
                return IsPromptVisibleNearCursor(scripting, promptPattern) && IsTerminalReadyPrompt(promptPattern);

            string[] parts = promptPattern.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rawPart in parts)
            {
                string part = rawPart.Trim();
                if (part.Length == 0) continue;

                if (IsTerminalReadyPrompt(part) && IsPromptVisibleNearCursor(scripting, part))
                    return true;
            }

            return false;
        }

        private static bool IsTerminalReadyPrompt(string promptToken)
        {
            if (string.IsNullOrWhiteSpace(promptToken))
                return false;

            string token = promptToken.Trim();
            if (token.EndsWith(":", StringComparison.Ordinal))
                return false;

            if (token.EndsWith("#", StringComparison.Ordinal) ||
                token.EndsWith(">", StringComparison.Ordinal) ||
                token.EndsWith("]", StringComparison.Ordinal) ||
                token.EndsWith("$", StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }

        private static string DetectReadyPromptNearCursor(Scripting scripting)
        {
            try
            {
                string relevantText = ReadTextAroundCursor(scripting.Terminal.Screen, 2);
                if (string.IsNullOrWhiteSpace(relevantText))
                    return string.Empty;

                string trimmed = relevantText.TrimEnd();
                if (trimmed.EndsWith("#", StringComparison.Ordinal)) return "#";
                if (trimmed.EndsWith(">", StringComparison.Ordinal)) return ">";
                if (trimmed.EndsWith("]", StringComparison.Ordinal)) return "]";
                if (trimmed.EndsWith("$", StringComparison.Ordinal)) return "$";
            }
            catch
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// 커서 기준 최근 n개 라인의 화면 텍스트만 읽어 프롬프트 검사 비용을 줄입니다.
        /// </summary>
        private static string ReadTextAroundCursor(TerminalScreen screen, int lookBackLines)
        {
            if (screen == null)
                return string.Empty;

            var sb = new StringBuilder();
            int cursorRow = screen.CursorTop;
            int startRow = Math.Max(0, cursorRow - lookBackLines);
            int endRow = cursorRow;

            for (int row = startRow; row <= endRow; row++)
            {
                for (int col = 0; col < screen.Columns; col++)
                {
                    TerminalCell cell = screen.GetCell(col, row);
                    char c = cell.Character;
                    sb.Append(c == '\0' ? ' ' : c);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string GetScreenExcerpt(Scripting scripting)
        {
            try
            {
                string text = ReadTextAroundCursor(scripting.Terminal.Screen, 5);
                if (string.IsNullOrWhiteSpace(text))
                    return string.Empty;

                return text.Replace("\r", " ").Replace("\n", " ").Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 프롬프트 문자열을 Rebex ScriptEvent로 변환합니다.
        /// | 가 포함된 경우 OR 조건의 복합 이벤트로 구성합니다.
        /// </summary>
        private static ScriptEvent BuildPromptEvent(string promptStr)
        {
            if (!promptStr.Contains("|"))
            {
                return ScriptEvent.FromString(promptStr);
            }

            string[] parts = promptStr.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return ScriptEvent.FromString(promptStr);

            ScriptEvent combinedEvent = ScriptEvent.FromString(parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                combinedEvent = combinedEvent | ScriptEvent.FromString(parts[i]);
            }

            return combinedEvent;
        }
    }
}
