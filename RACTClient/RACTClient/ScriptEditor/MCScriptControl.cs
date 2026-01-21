using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace RACTClient
{
    public class MCScriptControl
    {
        /// <summary>
        /// 스크립트 생성 완료 이벤트 입니다.
        /// </summary>
        public event DefaultHandler OnScriptMakeComplete;
        /// <summary>
        /// 스크립트 스트링 입니다.
        /// </summary>
        private string m_ScriptRawData = "";
        /// <summary>
        /// 스크립트 객체 입니다.
        /// </summary>
        private RACT m_ScriptObject = null;
        /// <summary>
        /// 스크립트에 사용할 스레드 입니다.
        /// </summary>
        private Thread m_ScriptProcessThread = null;
        /// <summary>
        /// Main Method 입니다.
        /// </summary>
        private string m_MainMethod = "";
        /// <summary>
        /// 실행 파라메터 입니다.
        /// </summary>
        private object[] m_Params;
        /// <summary>
        /// 스크립트 함수 목록 입니다.
        /// </summary>
        private Dictionary<string, List<string>> m_MethodList = new Dictionary<string, List<string>>();

        /// <summary>
        /// 실행할 스크립트를 설정 합니다.
        /// </summary>
        internal void ExecuteScript(string aScriptString)
        {
            m_ScriptRawData = aScriptString;
            m_MethodList.Clear();
            m_ScriptObject = null;
            if (m_ScriptProcessThread != null && m_ScriptProcessThread.IsAlive)
            {
                try
                {
                    m_ScriptProcessThread.Abort();
                }
                catch { }
            }

        }

        /// <summary>
        /// 스크립트에 사용할 객체를 설정 합니다.
        /// </summary>
        internal void AddObject(string aName, RACT aScriptObject)
        {
            m_ScriptObject = aScriptObject;
        }

        /// <summary>
        /// 스크립트를 실행 합니다.
        /// </summary>
        internal string Run(string aMethod, ref object[] oParams)
        {
            m_MainMethod = aMethod;
            m_Params = oParams;
            if (!ScriptParse()) return "";


            m_ScriptProcessThread = new Thread(new ThreadStart(RunScript));
            m_ScriptProcessThread.Start();


            return "OK";
        }
        /// <summary>
        /// 스크립트 처리 스레드를 실행 합니다.
        /// </summary>
        private void RunScript()
        {
            //서브 함수호출, 반복문, 조건문은 2차에서 하기로함 왜냐?? 시간 없으니깐 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ

            try
            {
                List<string> tScriptList = m_MethodList[m_MainMethod.ToUpper()];

                string tLine = "";
                string tScriptString = "";
                string[] tCommandSplit;
                int tKeyValue = -1;
                string tSendString = "TACT.SEND ";
                string tWaitString = "TACT.WAITFORSTRING ";
                string tSendCommand = "";

                for (int i = 0; i < tScriptList.Count; i++)
                {
                    tLine = tScriptList[i];

                    if (tLine.ToUpper().StartsWith(tSendString))
                    {
                        tScriptString = tLine.Substring(tSendString.Length);
                        tCommandSplit = tScriptString.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int tCommandIndex = 0; tCommandIndex < tCommandSplit.Length; tCommandIndex++)
                        {
                            if (tCommandSplit[tCommandIndex].Contains("\""))
                            {
                                tSendCommand += tCommandSplit[tCommandIndex].Replace("\"", "");
                            }
                            else if (tCommandSplit[tCommandIndex].Contains("(") && tCommandSplit[tCommandIndex].Contains(")"))
                            {
                                tKeyValue = int.Parse(tCommandSplit[tCommandIndex].Substring(tCommandSplit[tCommandIndex].IndexOf("(") + 1, tCommandSplit[tCommandIndex].IndexOf(")") - tCommandSplit[tCommandIndex].IndexOf("(") - 1));
                                tSendCommand += Convert.ToChar(tKeyValue);
                            }
                        }
                        m_ScriptObject.Send(tSendCommand);
                        tSendCommand = "";
                    }
                    else if (tLine.ToUpper().StartsWith(tWaitString))
                    {
                        //tScriptString = tScriptString = tLine.Substring(tWaitString.Length + 1);
                        tScriptString = tLine.Substring(tWaitString.Length + 1);
                        m_ScriptObject.WaitForString(tScriptString.Replace("\"", "").Trim());
                    }
                }

                if (OnScriptMakeComplete != null) OnScriptMakeComplete();
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(ex.ToString());
            }
        }

        /// <summary>
        /// 스크립트를 확인 합니다.
        /// </summary>
        /// <returns></returns>
        private bool ScriptParse()
        {
            System.Diagnostics.Debug.WriteLine("###### 스크립트를 확인 합니다. ######");

            string[] tSplitScript = m_ScriptRawData.Replace("\t","").Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            try
            {
                //1. 함수 형태를 확인 합니다.
                string tScriptLine = "";
                int tSubMethodCount = 0;
                int tMainMethodIndex = -1;
                string tMethodName = "";
                for (int i = 0; i < tSplitScript.Length; i++)
                {
                    tScriptLine = tSplitScript[i].Replace("\r","").Trim();
                    if (tScriptLine.Length == 0) continue;

                    //주석 제거
                    if (tScriptLine.StartsWith("'")) continue;

                    if (tScriptLine.ToUpper().StartsWith("SUB "))
                    {
                        tSubMethodCount++;
                        tMethodName = tScriptLine.ToUpper().Replace("SUB ", "").Trim();
                        if (m_MainMethod.ToUpper().Equals(tMethodName))
                        {
                            tMainMethodIndex = i;
                        }

                        m_MethodList.Add(tMethodName, new List<string>());
                    }
                    else if (tScriptLine.ToUpper().StartsWith("END SUB"))
                    {
                        tSubMethodCount--;
                    }
                    else
                    {
                        m_MethodList[tMethodName].Add(tScriptLine);
                    }

                }
                //함수 형식이 잘 못 되었습니다.
                if (tSubMethodCount != 0) return false;
                //실행 함수가 없습니다.
                if (tMainMethodIndex < 0) return false;
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "스크립트를 확인하세요.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 초기화 합니다.
        /// </summary>
        internal void Reset()
        {

        }
    }
}
