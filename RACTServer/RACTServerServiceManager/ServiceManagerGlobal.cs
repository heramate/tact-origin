using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using RACTCommonClass;
using System.Threading;
using RACTServer;
using RACTServerCommon;
using MKLibrary.Controls;
using System.Windows.Forms;
using System.Drawing;


namespace RACTServerServiceManager
{
    class ServiceManagerGlobal
    {
        /// <summary>
        /// 서비스 실행 여부 입니다.
        /// </summary>
        public static bool s_IsRun = false;
        /// <summary>
        /// 서버 시작 위치입니다.
        /// </summary>
        public static string m_ServerStartupPath = string.Empty;
        /// <summary>
        /// 서버 환경 설정이 저장된 XML 파일명입니다.
        /// </summary>
        public const string c_SystemConfigFileName = @"\SystemInfo.xml";
        /// <summary>
        /// DB 연결 정보 입니다.
        /// </summary>
        public static DBConnectionInfo m_DBConnectionInfo = null;
        /// <summary>
        /// 로그 저장 프로세서 입니다.
        /// </summary>
        public static FileLogProcess m_LogProcess = null;
        /// <summary>
        /// DB 로그 저장 프로세서 입니다.
        /// </summary>
        public static DBLogProcess m_DBLogProcess = null;
        /// <summary>
        /// 서버 환경 설정 정보입니다.
        /// </summary>
        public static SystemConfig m_SystemInfo = null;
        /// <summary>
        /// 요청 큐 입니다.
        /// </summary>
        public static Queue<CommunicationData> s_RequestQueue = new Queue<CommunicationData>();
        /// <summary>
        /// 요청자 목록 입니다.
        /// </summary>
        public static Dictionary<int, ISenderObject> s_SenderList = new Dictionary<int, ISenderObject>();
        /// <summary>
        /// 메인 폼 입니다.
        /// </summary>
        public static ServiceManagerMainForm s_MainForm;
        /// <summary>
        /// 서버에 접속 되었는지 여부 입니다.
        /// </summary>
        public static bool s_IsServerConnected;
        /// <summary>
        /// 리모트통신을 위한 원격객체입니다.
        /// </summary>
        public static MKRemote s_ServerRemoteGateway = null;
        /// <summary>
        /// 서버와 통신할 프로세서 입니다.
        /// </summary>
        public static ServerCommunicationProcess s_ServerCommunicationProcess = null;
        /// <summary>
        /// 현재 실행중인 Daemon 목록 입니다.
        /// </summary>
        public static DaemonProcessInfoCollection s_RunningDaemonList = null;

        private static ImageList ilCenter = null;
        private static ImageList ilLeft = null;
        private static ImageList ilRight = null;

        /// <summary>
        /// 버튼의 이미지 스타일을 설정합니다.
        /// </summary>
        /// <param name="aButtonType"></param>
        public static void SetButtonImageStyle()
        {
            if (ilCenter == null) ilCenter = new ImageList();
            if (ilLeft == null) ilLeft = new ImageList();
            if (ilRight == null) ilRight = new ImageList();

            ilCenter.TransparentColor = System.Drawing.Color.Transparent;
            ilLeft.TransparentColor = System.Drawing.Color.Transparent;
            ilRight.TransparentColor = System.Drawing.Color.Transparent;

            ilCenter.ColorDepth = ColorDepth.Depth32Bit;
            ilCenter.ImageSize = new Size(3, 27);

            ilLeft.ColorDepth = ColorDepth.Depth32Bit;
            ilLeft.ImageSize = new Size(5, 27);

            ilRight.ColorDepth = ColorDepth.Depth32Bit;
            ilRight.ImageSize = new Size(6, 27);

            ilCenter.Images.Clear();
            ilRight.Images.Clear();
            ilLeft.Images.Clear();

            Image tImage = global::RACTServerServiceManager.Properties.Resources.nomal_center;
            ilCenter.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.hover_center;
            ilCenter.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.click_center;
            ilCenter.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.disable_center;
            ilCenter.Images.Add(tImage);

            tImage = global::RACTServerServiceManager.Properties.Resources.nomal_left;
            ilLeft.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.hover_left;
            ilLeft.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.click_left;
            ilLeft.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.disable_left;
            ilLeft.Images.Add(tImage);

            tImage = global::RACTServerServiceManager.Properties.Resources.nomal_right;
            ilRight.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.hover_right;
            ilRight.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.click_right;
            ilRight.Images.Add(tImage);
            tImage = global::RACTServerServiceManager.Properties.Resources.disable_center;
            ilRight.Images.Add(tImage);
        }

        /// <summary>
        /// 버튼 스타일을 설정합니다.
        /// </summary>
        /// <param name="aButton"></param>
        public static void InitializeButtonStyle(MKButton aButton)
        {
            if (ilCenter == null) SetButtonImageStyle();

            aButton.ButtonImageCenter = ilCenter;
            aButton.ButtonImageLeft = ilLeft;
            aButton.ButtonImageRight = ilRight;

            MKImage tMKimage = new MKImage(0, 1, 2, 3);
            aButton.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(aButton, new MKImage[] { tMKimage, tMKimage, tMKimage });
            aButton.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Texture;
            aButton.ControlColorInfo.ForeColorDisable = Color.DarkGray;
            aButton.ControlColorInfo.ForeColorHover = Color.SteelBlue;
            aButton.ControlColorInfo.ForeColorNormal = Color.Black;
            aButton.ControlColorInfo.ForeColorSelect = Color.SteelBlue;

        }

        public static void SetSystemInfo(SystemConfig aSystemConfig)
        {
            // System 정보 저장
            ServiceManagerGlobal.m_SystemInfo = aSystemConfig;

            // DB 연결 정보 저장.
            ServiceManagerGlobal.m_DBConnectionInfo = new DBConnectionInfo();
            ServiceManagerGlobal.m_DBConnectionInfo.DBServerIP = ServiceManagerGlobal.m_SystemInfo.DBServerIP;
            ServiceManagerGlobal.m_DBConnectionInfo.DBName = ServiceManagerGlobal.m_SystemInfo.DBName;
            ServiceManagerGlobal.m_DBConnectionInfo.UserID = ServiceManagerGlobal.m_SystemInfo.UserID;
            ServiceManagerGlobal.m_DBConnectionInfo.Password = ServiceManagerGlobal.m_SystemInfo.Password;
            ServiceManagerGlobal.m_DBConnectionInfo.DBConnectionCount = ServiceManagerGlobal.m_SystemInfo.DBConnectionCount;
        }
    }
}
