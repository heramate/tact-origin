using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;

namespace RACTCommonClass
{
    /// <summary>
    /// Remote 통신을 위한 서버 부분 클래스 입니다.
    /// </summary>
    [Serializable]
    public class DaemonLauncherPartObject : MKRemoteObject
    {
        /// <summary>
        /// 실행 중인 Daemon 리스트를 요청할 핸들러 입니다.
        /// </summary>
        /// <returns></returns>
        public delegate byte[] GetRACTDaemonListHandler();
        /// <summary>
        /// 환경 저장시 사용할 핸들러 입니다.
        /// </summary>
        /// <returns></returns>
        public delegate bool SetLauncherConfigHandler(byte[] aConfig);
        /// <summary>
        /// Launcer Config를 요청할 핸들러 입니다.
        /// </summary>
        /// <returns></returns>
        public delegate byte[] GetLauncherConfigHandler();
        /// <summary>
        /// Daemon 실행 요청 합니다.
        /// </summary>
        /// <param name="aServerKey"></param>
        /// <returns></returns>
        public delegate bool SetStartServer(string aDaemonKey, bool aIsStart);

        /// <summary>
        /// Daemon List를 가져오기 합니다.
        /// </summary>
        private GetRACTDaemonListHandler m_GetRACTDaemonListHandlerMethod = null;
        /// <summary>
        /// 환경을 저장 합니다.
        /// </summary>
        private SetLauncherConfigHandler m_SetLauncherConfigHandlerMethod = null;
        /// <summary>
        /// Launcher Config를 가져오가 합니다.
        /// </summary>
        private GetLauncherConfigHandler m_GetLauncherConfigHandlerMethod = null;

        /// <summary>
        /// Daemon 실행 합니다.
        /// </summary>
        private SetStartServer m_SetStartServer = null;

    
      

        public void SetStartServerMethod(SetStartServer aInfo)
        {
            m_SetStartServer = aInfo;
        }

        public bool CallSetStartServerMethod(string aKey, bool aIsStart)
        {
            if (m_SetStartServer != null)
            {
                return m_SetStartServer(aKey, aIsStart);
            }
            return false;
        }

        /// <summary>
        /// Remote 통신을 위한 서버 부분 클래스 생성자 입니다.
        /// </summary>
        public DaemonLauncherPartObject() { }

        public void SetGetRACTDaemonLauncherConfigHandlerMethod(GetLauncherConfigHandler aInfo)
        {
            m_GetLauncherConfigHandlerMethod = aInfo;
        }
        /// <summary>
        /// Launcher config를 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        public byte[] CallGetRACTDaemonLauncherConfigHandlerMethod()
        {
            if (m_GetLauncherConfigHandlerMethod != null)
            {
                return m_GetLauncherConfigHandlerMethod();
            }
            return null;
        }

        /// <summary>
        /// 메소드를 등록 합니다.
        /// </summary>
        /// <param name="vUserLoginHandlerMethod"></param>
        public void SetGetRACTDaemonListHandlerMethod(GetRACTDaemonListHandler vUserLoginHandlerMethod)
        {
            m_GetRACTDaemonListHandlerMethod = vUserLoginHandlerMethod;
        }
        /// <summary>
        /// 환경 저장 메소드를 등록 합니다.
        /// </summary>
        /// <param name="aSetConfig"></param>
        public void SetSetLauncherConfigMethod(SetLauncherConfigHandler aSetConfig)
        {
            m_SetLauncherConfigHandlerMethod = aSetConfig;
        }
        /// <summary>
        /// 환경 저장 메소드를 호출 합니다.
        /// </summary>
        /// <param name="aConfig"></param>
        /// <returns></returns>
        public bool CallSetLauncherConfigHandlerMethod(byte[] aConfig)
        {
            if (m_SetLauncherConfigHandlerMethod != null)
            {
                return m_SetLauncherConfigHandlerMethod(aConfig);
            }
            return false;
        }

        /// <summary>
        /// Daemon 리스트를 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        public byte[] CallGetRACTDaemonListHandlerMethod()
        {
            if (m_GetRACTDaemonListHandlerMethod != null)
            {
                return m_GetRACTDaemonListHandlerMethod();
            }
            else
            {
                return null;
            }
        }
    }
}
