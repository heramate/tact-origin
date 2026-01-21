using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using System.Windows.Forms;

namespace RACTClient
{
    /// <summary>
    /// 스크립트 생성자 입니다.
    /// </summary>
    public class ScriptGenerator
    {
        /// <summary>
        /// 전송 목록 입니다.
        /// </summary>
        private Dictionary<int, TerminalScriptKeyInfo> m_SendScriptList = new Dictionary<int, TerminalScriptKeyInfo>();
        /// <summary>
        /// 대기 목록 입니다.
        /// </summary>
        private Dictionary<int, TerminalScriptKeyInfo> m_WaitScriptList = new Dictionary<int, TerminalScriptKeyInfo>();
        /// <summary>
        /// 스크립트 순서 입니다.
        /// </summary>
        private int m_ScriptSequence = 0;
        /// <summary>
        /// 전송 을 추가합니다.
        /// </summary>
        /// <param name="aInfo"></param>
        public void AddSend(TerminalScriptKeyInfo aInfo)
        {
            lock (m_SendScriptList)
            {
                aInfo.Sequence = m_ScriptSequence++;
                m_SendScriptList.Add(aInfo.Sequence,aInfo);
            }
        }
        /// <summary>
        /// 대기를 추가 합니다.
        /// </summary>
        /// <param name="aInfo"></param>
        internal void AddWait(TerminalScriptKeyInfo aInfo)
        {
            lock (m_WaitScriptList)
            {
                aInfo.Sequence = m_ScriptSequence++;
                m_WaitScriptList.Add(aInfo.Sequence,aInfo);
            }
        }
       
        /// <summary>
        /// 스크립트를 생성 합니다.
        /// </summary>
        /// <returns></returns>
        public string MakeScript()
        {
            StringBuilder tScript = new StringBuilder();
            string tTempString = "";
            string tTempLine = "";
            TerminalScriptKeyInfo tKeyInfo = null;
            int tControlKeyValue;
            E_ScriptLineType tLineType = E_ScriptLineType.Send;

            tScript.Append("'---------------------------------------\r\n");
            tScript.Append("'TACT Script v1.1 \r\n");
            tScript.Append("'생성 날짜 :"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") +" \r\n");
            tScript.Append("'---------------------------------------\r\n\r\n");

            tScript.Append("Sub Main \r\n");
            bool tEnterSave = false;

            for (int i = 0; i < m_ScriptSequence; i++)
            {
                if (i == 0) continue;
                if (m_SendScriptList.ContainsKey(i))
                {
                    tKeyInfo = m_SendScriptList[i];
                    tLineType = E_ScriptLineType.Send;
                }
                else
                {
                    tKeyInfo = m_WaitScriptList[i];
                    if (!tEnterSave)
                    {
                        tScript.Append("\t" + Script.s_Send + " \"" + tTempString + "\"\r\n");
                        tTempString = "";
                    }
                    tLineType = E_ScriptLineType.Wait;
                }

                if (tLineType == E_ScriptLineType.Send)
                {
                    if (tKeyInfo.Key.Equals("?"))
                    {
                       
                            tTempLine += "\"" + tTempString + tKeyInfo.Key + "\"";
                            tScript.Append("\t" + Script.s_Send + " " + tTempLine + "\r\n");
                            tTempString = "";
                            tTempLine = "";
                        
                    }
                    else
                    {
                        if (tKeyInfo.KeyType == E_TerminalScriptKeyType.Normal)
                        {
                            tTempString += tKeyInfo.Key;
                            tEnterSave = false;
                        }
                        else
                        {
                            if (tTempString.Length > 0)
                            {
                                tTempLine += "\"" + tTempString + "\"&";
                                tTempString = "";
                            }

                            tControlKeyValue = int.Parse(tKeyInfo.Key);

                            if (tControlKeyValue ==(int) Keys.Enter)
                            {
                                tTempLine += "chr(" + tKeyInfo.Key + ")";
                                tScript.Append("\t" + Script.s_Send + " " + tTempLine + "\r\n");
                                tTempLine = "";
                                tEnterSave = true;
                            }
                            else
                            {
                                tTempLine += "chr(" + tKeyInfo.Key + ")&";
                            }
                        }
                    }
                }
                else
                {
                    tScript.Append("\t" + Script.s_WaitForString + " \"" + tKeyInfo.Key + "\"\r\n");
                }
            }
            if (tTempString.Length > 0)
            {
                tScript.Append("\t" + Script.s_Send + " \"" + tTempString + "\"\r\n");
            }
            tScript.Append("End Sub");

            return tScript.ToString();
        }
        /// <summary>
        /// 스크립트를 초기화 합니다.
        /// </summary>
        internal void Reset()
        {
            m_SendScriptList.Clear();
            m_WaitScriptList.Clear();
            m_ScriptSequence = 0;
        }

        public static Script MakeDefaultConnectionCommand(FACT_DefaultConnectionCommandSet aCommandSet,DeviceInfo aDeviceInfo)
        {
            Script tCommandScript = null;
            try
            {
                StringBuilder tScript = new StringBuilder();
                string tTempString = "";
                string tTempLine = "";
                TerminalScriptKeyInfo tKeyInfo = null;
                int tControlKeyValue;
                E_ScriptLineType tLineType = E_ScriptLineType.Send;

                tScript.Append("'---------------------------------------\r\n");
                tScript.Append("'TACT Script v1.1 \r\n");
                tScript.Append("'생성 날짜 :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") + " \r\n");
                tScript.Append("'---------------------------------------\r\n\r\n");

                tScript.Append("Sub Main \r\n");

                //20170822 - NoSeungPil - RCCS 로그인의 경우 로그인전에 강제로 엔터키 전송
                if (AppGlobal.s_ConnectionMode == 1)
                {
                    tScript.Append("\t" + Script.s_WaitForString + " \"" + "+d" + "\"\r\n");
                    tScript.Append("\t" + Script.s_Send + " &chr(13)\r\n");
                }

                if ( AppGlobal.s_ConnectionMode == 2)
                {
                    tScript.Append("\t" + Script.s_WaitForString + " \"" + aCommandSet.CommandList[0].Prompt + "\"\r\n");
                }else
                    tScript.Append("\t" + Script.s_WaitForString + " \"" + aCommandSet.CommandList[0].Prompt + "\"\r\n");

                for (int i = 1; i < aCommandSet.CommandList.Count; i++)
                {
                    if (aCommandSet.CommandList[i].CMD.Equals("${USERID1}"))
                    {                        
                        tScript.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetID1 + "\"&chr(13)\r\n");
                        //2015-09-18 test hanjiyeon
                        //tScript.Append("\t" + Script.s_Send + " \"" + "\"&chr(13)\r\n");
                    }
                    else if (aCommandSet.CommandList[i].CMD.Equals("${PASSWORD1}"))
                    {
                        tScript.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetPwd1 + "\"&chr(13)\r\n");
                        //2015-09-18 test hanjiyeon
                        //tScript.Append("\t" + Script.s_Send + " \"" + "T" + "\"&chr(13)\r\n");
                    }
                    else if (aCommandSet.CommandList[i].CMD.Equals("${USERID2}"))
                    {
                        tScript.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetID2 + "\"&chr(13)\r\n");
                    }
                    else if (aCommandSet.CommandList[i].CMD.Equals("${PASSWORD2}"))
                    {
                        tScript.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetPwd2 + "\"&chr(13)\r\n");
                    }
                    else
                    {
                        tScript.Append("\t" + Script.s_Send + " \"" + aCommandSet.CommandList[i].CMD + "\"&chr(13)\r\n");
                    }

                    if (aCommandSet.CommandList[i].Prompt.Trim().Length > 0)
                    {
                        tScript.Append("\t" + Script.s_WaitForString + " \"" + aCommandSet.CommandList[i].Prompt + "\"\r\n");
                    }

                }

                tScript.Append("End Sub");

                tCommandScript = new Script(tScript.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                AppGlobal.s_FileLogProcessor.PrintLog(ex.ToString());
            }
            return tCommandScript;

        }

        public static Script MakeDefaultConnectionCommand_TL1(FACT_DefaultConnectionCommandSet aCommandSet, DeviceInfo aDeviceInfo)
        {
            Script tCommandScript = null;
            try
            {
                StringBuilder tScript = new StringBuilder();
                string tTempString = "";
                string tTempLine = "";
                TerminalScriptKeyInfo tKeyInfo = null;
                int tControlKeyValue;
                E_ScriptLineType tLineType = E_ScriptLineType.Send;

                tScript.Append("'---------------------------------------\r\n");
                tScript.Append("'TACT Script v1.1 \r\n");
                tScript.Append("'생성 날짜 :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") + " \r\n");
                tScript.Append("'---------------------------------------\r\n\r\n");

                tScript.Append("Sub Main \r\n");

                tScript.Append("\t" + Script.s_WaitForString + " \"" + aCommandSet.CommandList[0].TL1_Prompt + "\"\r\n");

                for (int i = 1; i < aCommandSet.CommandList.Count; i++)
                {
                    tScript.Append("\t" + Script.s_Send + " \"" + aCommandSet.CommandList[i].TL1_CMD + "\"&chr(13)\r\n");

                    if (aCommandSet.CommandList[i].TL1_Prompt.Trim().Length > 0)
                    {
                        tScript.Append("\t" + Script.s_WaitForString + " \"" + aCommandSet.CommandList[i].TL1_Prompt + "\"\r\n");
                    }

                }

                tScript.Append("End Sub");

                tCommandScript = new Script(tScript.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                AppGlobal.s_FileLogProcessor.PrintLog(ex.ToString());
            }
            return tCommandScript;

        }
		//2020-10-05 TACT기능개선 Script 일괄 명령실행 시, 일부Line 입력불가
		//배치 스크립트 형식으로 수행가능하도록 수정
		//일괄명령으로 전달되는 명령어를 스크립트 형식으로 만들어주는 함수
        public static Script MakeBatchCommand(string aValue1, string aPrompt)
        {
            Script tCommandScript = null;
            try
            {
                StringBuilder tScript = new StringBuilder();
                string tTempString = "";
                string tTempLine = "";
                TerminalScriptKeyInfo tKeyInfo = null;
                int tControlKeyValue;
                E_ScriptLineType tLineType = E_ScriptLineType.Send;
                String[] SepStrs = { "\r" };
                String[] CmdStr = aValue1.Split(SepStrs, StringSplitOptions.None);

                tScript.Append("'---------------------------------------\r\n");
                tScript.Append("'TACT Script v1.1 \r\n");
                tScript.Append("'생성 날짜 :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") + " \r\n");
                tScript.Append("'---------------------------------------\r\n\r\n");

                tScript.Append("Sub Main \r\n");

                //tScript.Append("\t" + Script.s_WaitForString + " \"" + ">|#" + "\"\r\n");

                for (int i = 0; i < CmdStr.Length; i++)
                {
                    String CurrentCmd = CmdStr[i].ToString();
                    if (CurrentCmd.Length <= 0)
                        continue;
                    tScript.Append("\t" + Script.s_Send + " \"" + CmdStr[i].ToString() + "\"&chr(13)\r\n");


                    tScript.Append("\t" + Script.s_WaitForString + " \"" + aPrompt + "\"\r\n");
                    
                }

                tScript.Append("End Sub");

                tCommandScript = new Script(tScript.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                AppGlobal.s_FileLogProcessor.PrintLog(ex.ToString());
            }
            return tCommandScript;

        }
    }


    public class TerminalScriptKeyInfo
    {

        /// <summary>
        /// 입력 키 값 입니다.
        /// </summary>
        private string m_Key;

        /// <summary>
        /// 키 타입 입니다.
        /// </summary>
        private E_TerminalScriptKeyType m_KeyType;

        /// <summary>
        /// Sequence number 입니다.
        /// </summary>
        private int m_Sequence;

        /// <summary>
        /// Sequence number 가져오거나 설정 합니다.
        /// </summary>
        public int Sequence
        {
            get { return m_Sequence; }
            set { m_Sequence = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aKey"></param>
        /// <param name="aType"></param>
        public TerminalScriptKeyInfo(string aKey, E_TerminalScriptKeyType aType)
        {
            m_Key = aKey;
            m_KeyType = aType;
        }

        /// <summary>
        /// 키 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_TerminalScriptKeyType KeyType
        {
            get { return m_KeyType; }
            set { m_KeyType = value; }
        }



        /// <summary>
        /// 입력 키 값 가져오거나 설정 합니다.
        /// </summary>
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

    }

    public enum E_TerminalScriptKeyType
    {
        /// <summary>
        /// 기본 키 입니다.
        /// </summary>
        Normal,
        /// <summary>
        /// 컨트롤키 입니다.
        /// </summary>
        Control
    }

    public enum E_ScriptLineType
    {
        Send,
        Wait
    }
}
