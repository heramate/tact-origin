using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using RACTCommonClass;
using System.Drawing;
using RACTSerialProcess;
using System.IO.Ports;
using System.Windows.Forms;

namespace RACTClient
{

    public class ClientOption
    {
        /// <summary>
        /// 접속 목록 변경시 발생할 이벤트 입니다.
        /// </summary>
        public event DefaultHandler OnConnectionHistoryChange;
        /// <summary>
        /// Server IP 입니다.
        /// </summary>
        //private string m_ServerIP = "127.0.0.1";

        //private string m_ServerIP = "192.168.10.3";
        //private string m_ServerIP = "118.217.79.48";
        private string m_ServerIP = "118.217.79.41";
        //private string m_ServerIP = "118.217.79.15";

        /// <summary>
        /// 클라이언트 메인 화면 입니다.
        /// </summary>
        private E_ClientDefaultMainControlType m_DefaultMainControl;
        /// <summary>
        /// 터미널 팝업 타입 입니다.
        /// </summary>
        private E_TerminalPopupType m_TerminalPopupType = E_TerminalPopupType.None;

        /// <summary>
        /// 2013-01-17 - shinyn - 메모장 팝업 타입입니다.
        /// </summary>
        private E_DefaultNotePadPopupType m_NotePadWindowsPopupType = E_DefaultNotePadPopupType.Tab;

        /// <summary>
        /// 단축 명령 대상 입니다.
        /// </summary>
        private E_ShortenCommandTagret m_ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;

        /// <summary>
        /// Log 파일 저장 위치 입니다.
        /// </summary>
        private string m_LogPath = Application.StartupPath + "\\Log\\";

        /// <summary>
        /// 스크립트 기본 저장 위치 입니다.
        /// </summary>
        private string m_ScriptSavePath = Application.StartupPath + "\\Script\\";

        /// <summary>
        /// 터미널 장비 자동 로그인 여부 입니다.
        /// </summary>
        private bool m_IsUseTerminalAutoLogin = true;

        /// <summary>
        /// 2014-07-08 세션 종료시 터미널 닫히는 옵션 처리
        /// </summary>
        private bool m_IsUseTerminalClose = false;

        /// <summary>
        /// 터미널 자동 MoreString 스크롤 여부 가져오거나 설정합니다.
        /// </summary>
        private bool m_IsUseTerminalAutoMoreString = true;

        /// <summary>
        /// 터미널 표시 이름 입니다.
        /// </summary>
        private E_TerminalDisplayNameType m_TerminalDisplayNameType = E_TerminalDisplayNameType.IPAddress;

        /// <summary>
        /// 터미널 팝업 타입 입니다.
        /// </summary>
        private E_DefaultTerminalPopupType m_TerminalWindowsPopupType = E_DefaultTerminalPopupType.Tab;

        /// <summary>
        /// Popup Windows Size 입니다.
        /// </summary>
        private int m_PopupSizeWidth = 653;
        private int m_PopupSizeHeight = 529;
        /// <summary>
        /// Popup Windows ColumnCount 입니다.
        /// </summary>
        private int m_TerminalColumnCount = 80;

        /// <summary>
        /// 터미널 배경 색 입니다.
        /// </summary>
        private Color m_TerminalBackGroundColor = Color.Black;

        /// <summary>
        /// 터미널 글자 색 입니다.
        /// </summary>
        private Color m_TerminalFontColor = Color.GreenYellow;

        /// <summary>
        /// 터미널 Bold 색 입니다.
        /// </summary>
        private Color m_TerminalBoldColor = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// 터미널 Blink 색 입니다.
        /// </summary>
        private Color m_TerminalBlinkColor = Color.Red;
        /// <summary>
        /// Font 입니다.
        /// </summary>
        private string m_TerminalFontName = "굴림체";

        /// <summary>
        /// Font Size 입니다.
        /// </summary>
        private float m_TerminalFontSize = 9f;

        /// <summary>
        /// Font Style 입니다.
        /// </summary>
        private FontStyle m_TerminalFontStyle = FontStyle.Regular;

        /// <summary>
        /// 터미널 배경 색 입니다.
        /// </summary>
        private Color m_HighlightBackGroundColor = Color.Black;
        /// <summary>
        /// 터미널 글자 색 입니다.
        /// </summary>
        private Color m_HighlightFontColor = Color.Red;
        /// <summary>
        /// 터미널 Bold 색 입니다.
        /// </summary>
        private Color m_HighlightBoldColor = Color.FromArgb(255, 255, 255);
        /// <summary>
        /// 터미널 Blink 색 입니다.
        /// </summary>
        private Color m_HighlightBlinkColor = Color.Red;

        /// <summary>
        /// Font 입니다.
        /// </summary>
        private string m_HighlightFontName = "굴림체";
        /// <summary>
        /// Font Size 입니다.
        /// </summary>
        private float m_HighlightFontSize = 9f;
        /// <summary>
        /// Font Style 입니다.
        /// </summary>
        private FontStyle m_HighlightFontStyle = FontStyle.Regular;

        /// <summary>
        /// 2019-01-23 터미널창의 Text 유지여부
        /// </summary>
        private bool m_IsTerminalTextClear = false;
        /// <summary>
        /// 2019-09-20 터미널 로그 자동 저장
        /// </summary>
        private bool m_IsAutoSaveLog = false;
        /// <summary>
        /// 2019-09-18 명령어 전송 지연
        /// </summary>
        private int m_SendDelay = 10;
        /// <summary>
        /// Serial Option 입니다.
        /// </summary>
        private TerminalConnectInfo m_ConnectionInfo;
        /// <summary>
        /// 연결 목록 최대 갯수 입니다.
        /// </summary>
        private int m_ConnectionHistoryMaxCount = 5;

        /// <summary>
        /// 연결 목록 입니다.
        /// </summary>
        private ArrayList m_ConnectionHistoryList = new ArrayList();


        /// <summary>
        /// Server IP 가져오거나 설정 합니다.
        /// </summary>
        public string ServerIP
        {
            get { return m_ServerIP; }
            set { m_ServerIP = value; }
        }
        /// <summary>
        /// 클라이언트 메인 화면 가져오거나 설정 합니다.
        /// </summary>
        public E_ClientDefaultMainControlType DefaultMainControl
        {
            get { return m_DefaultMainControl; }
            set { m_DefaultMainControl = value; }
        }
        /// <summary>
        /// 터미널 팝업 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_TerminalPopupType TerminalPopupType
        {
            get { return m_TerminalPopupType; }
            set { m_TerminalPopupType = value; }
        }
        /// <summary>
        /// 2013-01-17 - shinyn - 메모장 팝업 타입입니다.
        /// </summary>
        public E_DefaultNotePadPopupType NotePadWindowsPopupType
        {
            get { return m_NotePadWindowsPopupType; }
            set { m_NotePadWindowsPopupType = value; }
        }
        /// <summary>
        /// 단축 명령 대상 가져오거나 설정 합니다.
        /// </summary>
        public E_ShortenCommandTagret ShortenCommandTaget
        {
            get { return m_ShortenCommandTaget; }
            set { m_ShortenCommandTaget = value; }
        }
        /// <summary>
        /// Log 파일 저장 위치 가져오거나 설정 합니다.
        /// </summary>
        public string LogPath
        {
            get { return m_LogPath; }
            set { m_LogPath = value; }
        }
        /// <summary>
        /// 스크립트 기본 저장 위치 가져오거나 설정 합니다.
        /// </summary>
        public string ScriptSavePath
        {
            get { return m_ScriptSavePath; }
            set { m_ScriptSavePath = value; }
        }
        /// <summary>
        /// 터미널 장비 자동 로그인 여부 가져오거나 설정 합니다.
        /// </summary>
        public bool IsUseTerminalAutoLogin
        {
            get { return m_IsUseTerminalAutoLogin; }
            set { m_IsUseTerminalAutoLogin = value; }
        }
        /// <summary>
        /// 2014-07-08 세션 종료시 터미널 닫히는 옵션 처리
        /// </summary>
        public bool IsUseTerminalClose
        {
            get { return m_IsUseTerminalClose; }
            set { m_IsUseTerminalClose = value; }
        }
        /// <summary>
        /// 터미널 자동 MoreString 스크롤 여부 가져오거나 설정합니다.
        /// </summary>
        public bool IsUseTerminalAutoMoreString
        {
            get { return m_IsUseTerminalAutoMoreString; }
            set { m_IsUseTerminalAutoMoreString = value; }
        }
        /// <summary>
        /// 터미널 표시 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_TerminalDisplayNameType TerminalDisplayNameType
        {
            get { return m_TerminalDisplayNameType; }
            set { m_TerminalDisplayNameType = value; }
        }
        /// <summary>
        /// 터미널 팝업 타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_DefaultTerminalPopupType TerminalWindowsPopupType
        {
            get { return m_TerminalWindowsPopupType; }
            set { m_TerminalWindowsPopupType = value; }
        }
        /// <summary>
        /// Popup Windows Size 속성을 가져오거나 설정합니다.
        /// </summary>
        public int PopupSizeHeight
        {
            get { return m_PopupSizeHeight; }
            set { m_PopupSizeHeight = value; }
        }
        /// <summary>
        /// Popup Windows Size 속성을 가져오거나 설정합니다.
        /// </summary>
        public int PopupSizeWidth
        {
            get { return m_PopupSizeWidth; }
            set { m_PopupSizeWidth = value; }
        }
        /// <summary>
        /// Popup Windows ColumnCount 입니다.
        /// </summary>
        public int TerminalColumnCount
        {
            get { return m_TerminalColumnCount; }
            set { m_TerminalColumnCount = value; }
        }

        /// <summary>
        /// 터미널 배경 색 가져오거나 설정 합니다.
        /// </summary>
        public Color TerminalBackGroundColor
        {
            get { return m_TerminalBackGroundColor; }
            set { m_TerminalBackGroundColor = value; }
        }
        /// <summary>
        /// 터미널 글자 색 가져오거나 설정 합니다.
        /// </summary>
        public Color TerminalFontColor
        {
            get { return m_TerminalFontColor; }
            set { m_TerminalFontColor = value; }
        }
        /// <summary>
        /// 터미널 Bold 색 가져오거나 설정 합니다.
        /// </summary>
        public Color TerminalBoldColor
        {
            get { return m_TerminalBoldColor; }
            set { m_TerminalBoldColor = value; }
        }
        /// <summary>
        /// 터미널 Blink 색 가져오거나 설정 합니다.
        /// </summary>
        public Color TerminalBlinkColor
        {
            get { return m_TerminalBlinkColor; }
            set { m_TerminalBlinkColor = value; }
        }
        /// <summary>
        /// Font 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TerminalFontName
        {
            get { return m_TerminalFontName; }
            set { m_TerminalFontName = value; }
        }
        /// <summary>
        /// 폰트 크기 입니다.
        /// </summary>
        public float TerminalFontSize
        {
            get { return m_TerminalFontSize; }
            set { m_TerminalFontSize = value; }
        }
        /// <summary>
        /// Font Style 속성을 가져오거나 설정합니다.
        /// </summary>
        public FontStyle TerminalFontStyle
        {
            get { return m_TerminalFontStyle; }
            set { m_TerminalFontStyle = value; }
        }
        /// <summary>
        /// 중요 장비 강조 터미널 배경 색 가져오거나 설정 합니다.
        /// </summary>
        public Color HighlightBackGroundColor
        {
            get { return m_HighlightBackGroundColor; }
            set { m_HighlightBackGroundColor = value; }
        }
        /// <summary>
        /// 중요 장비 강조 터미널 글자 색 가져오거나 설정 합니다.
        /// </summary>
        public Color HighlightFontColor
        {
            get { return m_HighlightFontColor; }
            set { m_HighlightFontColor = value; }
        }
        /// <summary>
        /// 중요 장비 강조 터미널 Bold 색 가져오거나 설정 합니다.
        /// </summary>
        public Color HighlightBoldColor
        {
            get { return m_HighlightBoldColor; }
            set { m_HighlightBoldColor = value; }
        }
        /// <summary>
        /// 중요 장비 강조 터미널 Blink 색 가져오거나 설정 합니다.
        /// </summary>
        public Color HighlightBlinkColor
        {
            get { return m_HighlightBlinkColor; }
            set { m_HighlightBlinkColor = value; }
        }
        /// <summary>
        /// 중요 장비 강조 Font 속성을 가져오거나 설정합니다.
        /// </summary>
        public string HighlightFontName
        {
            get { return m_HighlightFontName; }
            set { m_HighlightFontName = value; }
        }

        /// <summary>
        /// 중요 장비 강조 폰트 크기 입니다.
        /// </summary>
        public float HighlightFontSize
        {
            get { return m_HighlightFontSize; }
            set { m_HighlightFontSize = value; }
        }
        /// <summary>
        /// Font Style 속성을 가져오거나 설정합니다.
        /// </summary>
        public FontStyle HighlightFontStyle
        {
            get { return m_HighlightFontStyle; }
            set { m_HighlightFontStyle = value; }
        }
        /// <summary>
        /// 터미널창의 Text 유지여부에 대한 속성을 가져오거나 설정합니다.
        /// </summary>
        public bool IsTerminalTextClear
        {
            get { return m_IsTerminalTextClear; }
            set { m_IsTerminalTextClear = value; }
        }
        /// <summary>
        /// 2019-09-20 터미널 로그 자동 저장
        /// </summary>
        public bool IsAutoSaveLog
        {
            get { return m_IsAutoSaveLog; }
            set { m_IsAutoSaveLog = value; }
        }
        /// <summary>
        /// 2019-09-18 명령어 전송 지연
        /// </summary>
        public int SendDelay
        {
            get { return m_SendDelay; }
            set { m_SendDelay = value; }
        }
        /// <summary>
        /// Serial Option 가져오거나 설정 합니다.
        /// </summary>
        public TerminalConnectInfo ConnectionInfo
        {
            get { return m_ConnectionInfo; }
            set { m_ConnectionInfo = value; }
        }
        /// <summary>
        /// 연결 목록 최대 갯수 가져오거나 설정 합니다.
        /// </summary>
        public int ConnectionHistoryMaxCount
        {
            get { return m_ConnectionHistoryMaxCount; }
            set { m_ConnectionHistoryMaxCount = value; }
        }
        /// <summary>
        /// 연결 목록 가져오거나 설정 합니다.
        /// </summary>
        public ArrayList ConnectionHistoryList
        {
            get { return m_ConnectionHistoryList; }
            set { m_ConnectionHistoryList = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ClientOption()
        {
            m_ConnectionInfo = new TerminalConnectInfo();
        }
        
        /// <summary>
        /// 연결 목록을 추가합니다.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        internal void AddConnectionHistory(DeviceInfo aDeviceInfo)
        {
            //목록에 존재 하는지 확인
            int tIndex = -1;
            string tDisplayName = "";

            if (aDeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                tDisplayName = aDeviceInfo.IPAddress;
            }
            else if (aDeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                tDisplayName = aDeviceInfo.IPAddress;
            }
            else
            {
                tDisplayName = aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
            }

            ConnectionHistoryInfo tHistory;
            for (int i = 0; i < m_ConnectionHistoryList.Count; i++)
            {
                tHistory = (ConnectionHistoryInfo)m_ConnectionHistoryList[i];

                if (tHistory.ConnectionInfo.ConnectionProtocol == aDeviceInfo.TerminalConnectInfo.ConnectionProtocol)
                {
                    if (tHistory.ConnectionInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                    {
                        if (tHistory.ConnectionInfo.IPAddress.Equals(tDisplayName))
                        {
                            tIndex = i;
                            break;
                        }
                    }
                    else if (tHistory.ConnectionInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                        if (tHistory.ConnectionInfo.IPAddress.Equals(tDisplayName))
                        {
                            tIndex = i;
                            break;
                        }
                    }
                    else
                    {
                        if (tHistory.ConnectionInfo.SerialConfig.PortName.Equals(tDisplayName))
                        {
                            tIndex = i;
                            break;
                        }
                    }
                }
            }

            if (tIndex > -1)
            {
                m_ConnectionHistoryList.RemoveAt(tIndex);
            }
            else
            {
                if (m_ConnectionHistoryList.Count >= m_ConnectionHistoryMaxCount)
                {
                    m_ConnectionHistoryList.RemoveAt(0);
                }
            }
            m_ConnectionHistoryList.Add(new ConnectionHistoryInfo(aDeviceInfo.TerminalConnectInfo));

            if (OnConnectionHistoryChange != null) OnConnectionHistoryChange();
        }

        



    }
}
