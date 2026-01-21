using System;
using System.Collections.Generic;
using System.Text;

using RACTCommonClass;
using System.Windows.Forms;
using MKLibrary.MKNetwork;
using System.Threading;
using System.Collections;
using MKLibrary.Controls;
using C1.Win.C1FlexGrid;
using System.Drawing;
using System.IO.Compression;
using MKLibrary.MKData;
using RACTTerminal;
using RACTSerialProcess;
using System.IO;
using System.Xml;
using System.Linq;

namespace RACTClient
{

    /// <summary>
    /// 2013-04-19 - shinyn - 그리드에서 복사 붙여넣기시 사용합니다.
    /// </summary>
    public enum E_ClipboardProcessType
    {
        /// <summary>
        /// 처리 없음 입니다.
        /// </summary>
        None,
        /// <summary>
        /// 복사 처리 입니다.
        /// </summary>
        Copy,
        /// <summary>
        /// 붙여넣기 처리 입니다.
        /// </summary>
        Paste
    }

    public class EncryptGlobal
    {
        public static string s_shaYN = "0";
    }

    public class AppGlobal
    {

        public static TerminalPanel m_TerminalPanel = null;

        /// <summary>
        /// 접근제어 실행 모드 입니다.
        /// </summary>
        public static E_RACTClientMode s_RACTClientMode = E_RACTClientMode.Online;
        /// <summary>
        /// 접근제어 시스템을 호출한 시스템 타입 입니다.
        /// </summary>
        public static E_TerminalMode s_Caller = E_TerminalMode.RACTClient;
        /// <summary>
        /// 직접실행인지 구분하기 위한 사용자 계정
        /// </summary>
        public static bool m_DirectConnect = false;

        public static string[] s_FontList = new string[] {"굴림체"
                                                        ,"돋움체"
                                                        ,"궁서체"
                                                        ,"바탕체"
                                                        ,"Arial"
                                                        ,"Calibri"
                                                        ,"Candara"
                                                        ,"Century"
                                                        ,"Constantia"
                                                        ,"MS Mincho"};
        /// <summary>
        /// 클라이언트 IP 입니다.
        /// </summary>
        public static string s_ClientIP = "";
        /// <summary>
        /// 레이아웃 파일 이름 입니다.
        /// </summary>
        public static string s_LayOutFileName = "\\RACTLayout.xml";
        /// <summary>
        /// 버전 정보 입니다.
        /// </summary>
        //public static string s_Version = "1.1.2.10";
        // 2013-02-21 - shinyn - TACT 버전변경 (고도화버전.기능추가.버그수정.릴리즈횟수)
        //public static string s_Version = "2.1.0.0";
        //public static string s_Version = "2.2.0.0"; //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
        //public static string s_Version = "2.2.1.0"; //2017.08.04 - KwonTaeSuk - 드래그로 영역선택시 자동스크롤 기능 추가
        //public static string s_Version = "2.2.2.0"; //20170818 - NoSeungPil - RCCS 로그인의 경우 종료시 강제로 ctrl + d 전송
        //public static string s_Version = "2.2.3.0"; //20170822 - NoSeungPil - RCCS 로그인의 경우 로그인전에 강제로 엔터키 전송
        //public static string s_Version = "2.3.0.0"; //20190212 고도화 개발(보안관리기능,RPCS 원격관리접속)
        //public static string s_Version = "2.3.0.1"; //20190326 고도화 개발(보안관리기능 명령어제한 기능 보안,불편 및 개선사항,장비접근 로그인,로그아웃시각 DB기준으로 변경)
        //public static string s_Version = "2.3.0.2"; //20190701 고도화 개발(불편 및 개선사항,RPCS 접속구분 추가)
        //public static string s_Version = "2.3.0.3"; //20190930 고도화 개발(불편/개선사항 외 OneTerminal 수행시 상태 표시 바,OneTerminal 옵션 메뉴 제공,자동저장 스위치 옵션항목으로 포함 기능 제공,집선스위치, G-PON-OLT, NG-PON-OLT 접속시 중요장비 표시(FONT COLOR RED적용)건 개선)
        //public static string s_Version = "2.3.0.4"; //20191007 집선스위치, G-PON-OLT, NG-PON-OLT 접속시 중요장비 표시(FONT COLOR RED적용)건 변경)
        //public static string s_Version = "2.3.0.5"; //20191110  명령어 전송 딜레이 설정 기능,Log 경로 설정 개선
        //public static string s_Version = "2.3.0.6"; //20191118 LTE 터널링 요청 반복 개선(한번 요청하고 응답없음면 연결 종료),텔넷 스레드 카운트 1->3
        //public static string s_Version = "2.3.0.7"; //20191209 명령어 전송 딜레이 설정 기능 수정, 첫 접속시 Ctrl+C,V 시 프롬트트 인식 오류 수정
        //public static string s_Version = "2.4.0.0"; // 2020-10-08 KwonTaeSUk [20고도화(.NET업그레이드)] .NET Framework 2.0 -> 4.8 (minor+1)
        //public static string s_Version = "2.4.0.1"; // 2021-04-15 KANGBONGHAN 일괄명령창 개선 단일명령어,복수명령어 수행 방식을 분리
        //public static string s_Version = "2.4.0.2"; // 2021-04-21 터미널 라인 갱신 안되는 부분 개선 // 2021-07-07KANGBONGHAN OneTerminal 자동스크롤 관련 개선
        //public static string s_Version = "2.4.0.3"; // 2021-11-04 터미널 컬럼수 사이즈 고정(80) 항목 삭제
        //public static string s_Version = "2.4.0.4";   // 2022-01-12 RPCS 무선접속 모델 추가 DSW105PR
        //public static string s_Version = "2.4.0.5";    //2022-08-18 Console모드 시리얼연결시 붙여넣기 오류 확인 및 개선
        //public static string s_Version = "2.4.0.6";    //2022-09-01 우회접속 보안이슈 개선(Online모드에서 접속시(Telnet) TACT데몬을 통해 연결
        //public static string s_Version = "2.4.0.7";    //2023-02-07 Online모드에서 접속시(ssh) TACT데몬을 통해 연결 개선
        //public static string s_Version = "2.4.0.8";     //2023-02-24 명령어 첫글자 누락 또는 붙여넣기시 끊김,첫글자 누락 수정 개선
        //public static string s_Version = "2.4.0.9";   //2023-06 AGW 2001포트 변경건
        //public static string s_Version = "2.4.1.0";   //2024-03 찾기( Ctrl + F) 기능 사용 후 명령어 띄어쓰기 불가, 프롬프트() 이슈
        //public static string s_Version = "2.4.1.1";   //2025-01-20 More 처리시 H모델 같은 라인 두줄 출력되는 현상 개선
        //public static string s_Version = "2.4.2.0";   //2025-03-12 CATV 관련 적용
        //public static string s_Version = "2.4.2.1";   //2025-05-14 IP검색 조건 추가(유효성 추가, 3옥텟 이상만 조회 3번째 자리 까지만 값 존재시 Like, 4번째 자리까지 있을 경우 일치로)
        //public static string s_Version = "2.4.2.2";   //2025-06-18 보안 이슈로 직접실행 금지 
        //public static string s_Version = "2.4.2.3";   //2025-07-04 직접실행시 Console모드 오픈 
        public static string s_Version = "2.4.2.4";   //2025-08-04 V4604S 접속 포트 23관련 수정

        /// <summary>
        /// 로그인 결과 입니다.
        /// </summary>
        public static LoginResultInfo s_LoginResult;
        /// <summary>
        /// 메인 폼 입니다.
        /// </summary>
        public static Form s_ClientMainForm;
        /// <summary>
        /// 리모트통신을 위한 원격객체입니다.
        /// </summary>
        public static MKRemote s_RemoteGateway = null;
        /// <summary>
        /// 서버 IP 입니다.
        /// </summary>
        //public static string s_ServerIP = "118.217.79.48";
        public static string s_ServerIP = "118.217.79.41";
        //public static string s_ServerIP = "118.217.79.15";
        //public static string s_ServerIP = "192.168.10.3";

        //public static string s_ServerIP = "127.0.0.1";
        /// <summary>
        /// 서버 Port 입니다.
        /// </summary>
        public static int s_ServerPort = 43210;
        /// <summary>
        /// 프로그램 이름 입니다.
        /// </summary>
        public static string s_ProgramName = "TACT Client";
        /// <summary>
        /// Mac Address 입니다.
        /// </summary>
        public static string s_MacAddress = "";
        /// <summary>
        /// 서버 접속 여부 입니다.
        /// </summary>
        public static bool s_IsServerConnected = false;
        /// <summary>
        /// 서버 데이터 검사 주기 입니다(단위 : 밀리초)
        /// </summary>
        public static int s_ServerCheckInterval = 100;
        /// <summary>
        /// 요청 큐 입니다.
        /// </summary>
        public static Queue<CommunicationData> s_RequestQueue = new Queue<CommunicationData>();

        /// <summary>
        /// 2013-05-03-shinyn - 선택된 접근권한 노드입니다.
        /// </summary>
        public static TreeNodeEx m_SelectedSystemNode = null;

        /// <summary>
        /// 2013-09-09- shinyn - 선택된 사용자 노드입니다.
        /// </summary>
        public static TreeNodeEx m_SelectedUserNode = null;



        /// <summary>
        /// 사용자 계정
        /// </summary>
        public static string s_UserAccount;
        /// <summary>
        /// 사용자 비밀번호
        /// </summary>
        public static string s_Password;
        /// <summary>
        /// 채널 이름 입니다.
        /// </summary>
        public static string s_ChannelName = "RemoteClient";
        /// <summary>
        /// 요청자 목록 입니다.
        /// </summary>
        public static Dictionary<int, ISenderObject> s_SenderList = new Dictionary<int, ISenderObject>();
        /// <summary>
        /// 사용자 그룹 목록 입니다.
        /// </summary>
        public static GroupInfoCollection s_GroupInfoList = new GroupInfoCollection();

        /// <summary>
        /// 2013-08-14- shinyn -사용자리스트의 사용자 그룹 목록입니다.
        /// </summary>
        public static UserInfoCollection s_UserInfoList = new UserInfoCollection();


        /// <summary>
        /// 단축 명령 목록 입니다.
        /// </summary>
        public static ShortenCommandGroupInfoCollection s_ShortenCommandList = new ShortenCommandGroupInfoCollection();
        /// <summary>
        /// 스크립트 목록 입니다.
        /// </summary>
        public static ScriptGroupInfoCollection s_ScriptList = new ScriptGroupInfoCollection();
        /// <summary>
        /// 데이터 동기 처리 입니다.
        /// </summary>
        public static DataSyncProcessor s_DataSyncProcssor = new DataSyncProcessor();
        /// <summary>
        /// 모델 목록 입니다.
        /// </summary>
        public static ModelInfoCollection s_ModelInfoList = new ModelInfoCollection();
        /// <summary>
        /// 제한 명령어 목록 입니다.
        /// </summary>
        public static LimitCmdInfoCollection s_LimitCmdInfoList = new LimitCmdInfoCollection();
        /// <summary>
        /// 기본 명령어 목록 입니다.
        /// </summary>
        public static DefaultCmdInfoCollection s_DefaultCmdInfoList = new DefaultCmdInfoCollection();
        /// <summary>
        /// 자동완성  목록 입니다.
        /// </summary>
        public static AutoCompleteCmdInfoCollection s_AutoCompleteCmdList = new AutoCompleteCmdInfoCollection();
        /// <summary>
        /// 장비 구분 목록입니다.
        /// </summary>
        public static ArrayList s_DevicePartList = new ArrayList();
        /// <summary>
        /// 장비정보 목록 입니다.
        /// </summary>
        //public static DeviceInfoCollection s_DeviceInfoList = new DeviceInfoCollection();
        /// <summary>
        /// 클라이언트 옵션 정보 입니다.
        /// </summary>
        public static ClientOption s_ClientOption = null;
        /// <summary>
        /// 조직 정보 입니다.
        /// </summary>
        public static OrganizationInfo s_OrganizationInfo;
        /// <summary>
        /// 찾기 폼 입니다.
        /// </summary>
        // public static TelnetFindForm  s_TelnetFindForm = null;
        /// <summary>
        /// 터미널 로그 처리 프로세서 입니다.
        /// </summary>
        public static CommandExecuteLogProcess s_TerminalExecuteLogProcess;
        /// <summary>
        /// 텔넷 데몬 목록 입니다.
        /// </summary>
        public static Dictionary<int, DaemonProcessRemoteObject> s_DaemonProcessList = new Dictionary<int, DaemonProcessRemoteObject>();
        /// <summary>
        /// Serial 처리 프로세서 입니다.
        /// </summary>
        public static SerialProcess s_SerialProcessor;
        /// <summary>
        /// 텔넷 처리 프로세서 입니다.
        /// </summary>
        public static TelnetProcessor.TelnetProcessor s_TelnetProcessor;
        /// <summary>
        /// 모드 변경으로 접속 할 경우인지 여부 입니다.
        /// </summary>
        public static bool s_IsModeChangeConnect = false;
        /// <summary>
        /// 모드 변경 폼 입니다.
        /// </summary>
        public static ModeChangeSubForm s_ModeChangeForm = new ModeChangeSubForm();
        /// <summary>
        /// 파일 로그 프로세서 입니다.
        /// </summary>
        public static FileLogProcess s_FileLogProcessor = null;
        /// <summary>
        /// 요청 대기 타임 아웃 입니다.
        /// </summary>
        public static readonly int s_RequestTimeOut = 5000;
        /// <summary>
        /// 프로그램 종료 여부 입니다.
        /// </summary>
        public static bool s_IsProgramShutdown = false;

        /// <summary>
        /// 자동저장 여부 입니다.
        /// </summary>
        public static bool s_IsAutoSaveLog = false;

        //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
        /// <summary>
        /// 접속방법 입니다. (1:RCCS 로그인)
        /// </summary>
        public static int s_ConnectionMode = 0;

        /// <summary>
        /// RCCS 로그인 체크
        /// </summary>
        public static bool s_IsRCCSLoginOK = false;

        /// <summary>
        /// RCCS 장비의 IP 입니다.
        /// </summary>
        public static string s_RCCSIP;

        /// <summary>
        /// RCCS 장비의 접속 Port번호 입니다.
        /// </summary>
        public static int s_RCCSPort = 0;

        /// <summary>
        /// RPCS 장비의 IP 입니다.
        /// </summary>
        public static string s_RPCSIP;

        /// <summary>
        /// RPCS 장비의 접속 Port번호 입니다.
        /// </summary>
        public static int s_RPCSPort = 0;

        public static int s_MultipleCmd = 20;

        public static E_IpType m_ViewIPType = E_IpType.ALL;

        /// <summary>
        /// 헬스 체크를 처리합니다.
        /// </summary>
        /// <returns></returns>
        public static bool AreYouThere()
        {
            return true;
        }

        /// <summary>
        /// 로그인 및 서버 연결을 처리 합니다.
        /// </summary>
        /// <param name="vID">사용자 아이디 입니다.</param>
        /// <param name="vPwd">사용자 패스워드 입니다.</param>
        /// <param name="vIPAddress">사용자 아이피 주소 입니다.</param>
        public static bool LoginConnect()
        {
            string tLogMessage = "";

            try
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat(AppGlobal.s_UserAccount, " 계정으로 로그인 합니다."));
                RemoteClientMethod tSPO = (RemoteClientMethod)AppGlobal.s_RemoteGateway.ServerObject;

                // 2014-03-19 - 신윤남 - 웹에서 실행해서 로그인시 DB에서 sha256으로 암호화된 비밀번호로 로그인되도록 하고,
                // 아이디,비밀번호 입력해서 로그인시 비밀번호를 sha256으로 암호화하여 로그인되도록 수정
                string password = Hash.GetHashPW(AppGlobal.s_Password);
                if (EncryptGlobal.s_shaYN == "1")
                {
                    password = AppGlobal.s_Password;
                    EncryptGlobal.s_shaYN = "0";
                }
                //20131118 김도균 패스워드를 SHA-256 UTF-8 방식으로 넘김
                s_LoginResult = (LoginResultInfo)ObjectConverter.GetObject(tSPO.CallUserLoginMethod(AppGlobal.s_UserAccount, password, AppGlobal.s_ClientIP, AppGlobal.s_Caller));
                if (s_ClientMainForm is ClientMain)
                {
                    ((ClientMain)s_ClientMainForm).SetMainFormText("TACT 클라이언트 (Ver " + AppGlobal.s_Version + ") :::");// + AppGlobal.m_ServerIP + " (서버 Ver " + m_LoginResult.ServerVersion + ")");
                }


                if (s_LoginResult.LoginResult != E_LoginResult.Success)
                {
                    switch (AppGlobal.s_LoginResult.LoginResult)
                    {
                        case E_LoginResult.IncorrectID:
                            AppGlobal.ShowMessage("계정을 확인해 주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "계정을 확인해 주세요. ");
                            break;
                        case E_LoginResult.IncorrectPassword:
                            AppGlobal.ShowMessage("비밀번호를 확인해 주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "비밀번호를 확인해 주세요. ");
                            //Application.Exit();
                            break;
                        case E_LoginResult.AlreadyLogin:
                            if (s_Caller == E_TerminalMode.RACTClient)
                            {
                                AppGlobal.ShowMessage("같은 계정이 이미 로그인 되어 있습니다. 클라이언트를 강제 종료 하였을경우 30초 이후에 다시 접속 하십시오.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "이미 로그인 되어 있습니다. ");

                                // AppGlobal.s_ClientMainForm.Close();
                                break;
                            }
                            else
                            {
                                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "로그인에 " + E_LoginResult.Success.ToString() + "했습니다.");
                                AppGlobal.s_IsServerConnected = true;
                                return true;
                            }
                        case E_LoginResult.UnknownError:
                            AppGlobal.ShowMessage("정보가 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "정보가 없습니다.");
                            //Application.Exit();
                            break;
                        case E_LoginResult.NotAuthentication:
                            AppGlobal.ShowMessage("TACT 사용 권한이 없습니다.\n관리자에게 문의 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TACT 사용 권한이 없습니다.");
                            // Application.Exit();
                            break;
                        case E_LoginResult.UnUsedLimit:
                            AppGlobal.ShowMessage("TACT 접속 제한 날짜가 지났습니다.\n관리자에게 문의 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TACT 접속 제한 날짜가 지났습니다..");
                            break;
                        default:
                            AppGlobal.ShowMessage("중대한 오류가 발생 햇습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "중대한 오류가 발생 햇습니다.");
                            // Application.Exit();
                            break;
                    }
                    Application.ExitThread();
                    // Application.Exit();
                    return false;
                }
                else
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "로그인에 " + s_LoginResult.LoginResult.ToString() + "했습니다.");
                    AppGlobal.s_IsServerConnected = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "서버와 연결할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
            return false;
        }




        /// <summary>
        /// 데몬 서버에 연결을 시도 합니다.
        /// </summary>
        /// <returns>연결 시도 성공 여부 입니다.</returns>
        public static E_ConnectError TryDaemonConnect(string aDaemonIP, int aDaemonPort, string aDaemonChannelName, out MKRemote oRemoteObject)
        {
            int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;
            MKRemote tRemote;
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat("Daemon(", aDaemonIP, ":", aDaemonPort, ":", aDaemonChannelName, ") 에 접속 합니다."));
            oRemoteObject = new MKRemote(E_RemoteType.TCPRemote, aDaemonIP, aDaemonPort, aDaemonChannelName);


            if (oRemoteObject == null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("Daemon(", aDaemonIP, ":", aDaemonPort, ":", aDaemonChannelName, ") 에 접속 할 수 없습니다."));
                return E_ConnectError.LocalFail;
            }
            else
            {
                while (!AppGlobal.s_IsProgramShutdown)
                {
                    try
                    {
                        tTryCount++;
                        if (tTryCount > 5)
                        {
                            return E_ConnectError.ServerNoRun;
                        }

                        if (oRemoteObject.ConnectServer(out tErrorString) != E_RemoteError.Success)
                        {
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("Daemon에 연결할 수 없습니다. Daemon이 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오. :", tErrorString));
                            return E_ConnectError.LinkFail;
                        }

                        tErrorString = string.Empty;

                        tSPO = (RemoteClientMethod)oRemoteObject.ServerObject;
                        if (tSPO == null)
                        {
                            Thread.Sleep(3000);
                            continue;
                        }
                        ObjectConverter.GetObject(tSPO.CallResultMethod(0));
                        break;
                    }
                    catch (Exception ex)
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
                        if (((TimeSpan)DateTime.Now.Subtract(tSDate)).TotalSeconds > 60)
                        {
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("Daemon에 연결할 수 없습니다. Daemon이 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오. :", tErrorString));
                            return E_ConnectError.LinkFail;
                        }
                    }
                }

                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat("Daemon(", aDaemonIP, ":", aDaemonPort, ":", aDaemonChannelName, ") 에 접속 했습니다."));
                return E_ConnectError.NoError;
            }
        }

        /// <summary>
        /// 서버에 연결을 시도 합니다.
        /// </summary>
        /// <returns>연결 시도 성공 여부 입니다.</returns>
        public static E_ConnectError TryServerConnect()
        {
            int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;

            if (s_RemoteGateway == null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat("서버(", s_ServerIP, ":", s_ServerPort, ")에 연결 합니다."));
                s_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, s_ServerIP, s_ServerPort, s_ChannelName);
                //test code
                // s_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, "192.168.25.4", s_ServerPort, s_ChannelName);
            }

            if (s_RemoteGateway == null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("서버(", s_ServerIP, ":", s_ServerPort, ")에 연결 할 수 없습니다."));
                return E_ConnectError.LocalFail;
            }
            else
            {
                while (!AppGlobal.s_IsProgramShutdown)
                {
                    try
                    {
                        tTryCount++;
                        if (tTryCount > 5)
                        {
                            return E_ConnectError.ServerNoRun;
                        }

                        if (AppGlobal.s_RemoteGateway.ConnectServer(out tErrorString) != E_RemoteError.Success)
                        {
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "서버에 연결할 수 없습니다. 서버가 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오.");
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, tErrorString);
                            return E_ConnectError.LinkFail;
                        }

                        tErrorString = string.Empty;

                        tSPO = (RemoteClientMethod)AppGlobal.s_RemoteGateway.ServerObject;
                        if (tSPO == null)
                        {
                            Thread.Sleep(3000);
                            continue;
                        }
                        ObjectConverter.GetObject(tSPO.CallResultMethod(0));
                        break;
                    }
                    catch (Exception ex)
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
                        if (((TimeSpan)DateTime.Now.Subtract(tSDate)).TotalSeconds > 60)
                        {
                            return E_ConnectError.LinkFail;
                        }
                    }
                }
                return E_ConnectError.NoError;
            }
        }


        /// <summary>
        /// 공용 메시지 박스입니다.
        /// </summary>
        public static DialogResult ShowMessage(string vMessage, MessageBoxButtons vButtonType, MessageBoxIcon vIconType)
        {
            DialogResult result = MessageBox.Show(vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType);
            return result;
        }

        /// <summary>
        /// 공용 메시지 박스입니다.
        /// </summary>
        public static DialogResult ShowMessage(System.Windows.Forms.IWin32Window vForm, string vMessage, MessageBoxButtons vButtonType, MessageBoxIcon vIconType)
        {
            Form tForm = (Form)vForm;

            if (tForm == null)
            {
                return MessageBox.Show(vForm, vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType);
            }

            if (tForm.InvokeRequired)
            {
                return (DialogResult)tForm.Invoke(new HandlerArgument4<Form, string, MessageBoxButtons, MessageBoxIcon>(ShowMessageBox),
                    new object[] { vForm, vMessage, vButtonType, vIconType });
            }
            else
            {
                return MessageBox.Show(vForm, vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType);
            }
        }

        public static DialogResult ShowMessageBox(Form vForm, string vMessage, MessageBoxButtons vButtonType, MessageBoxIcon vIconType)
        {
            return MessageBox.Show(vForm, vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType);

        }

        /// <summary>
        /// 공용 메시지 박스입니다.
        /// </summary>
        public static DialogResult ShowMessage(System.Windows.Forms.IWin32Window vForm, string vMessage, MessageBoxButtons vButtonType, MessageBoxIcon vIconType
            , MessageBoxDefaultButton aDefaultButton)
        {
            Form tForm = (Form)vForm;

            if (tForm == null)
            {
                return MessageBox.Show(vForm, vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType, aDefaultButton);
            }

            if (tForm.InvokeRequired)
            {
                return (DialogResult)tForm.Invoke(new HandlerArgument5<Form, string, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton>(ShowMessageBox),
                    new object[] { vForm, vMessage, vButtonType, vIconType, aDefaultButton });
            }
            else
            {
                return MessageBox.Show(vForm, vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType, aDefaultButton);
            }
        }

        public static DialogResult ShowMessageBox(Form vForm, string vMessage, MessageBoxButtons vButtonType, MessageBoxIcon vIconType
            , MessageBoxDefaultButton aDefaultButton)
        {
            return MessageBox.Show(vForm, vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType, aDefaultButton);
        }

        /// <summary>
        /// 공용 메시지 박스입니다.
        /// </summary>
        public static DialogResult ShowMessage(string vMessage)
        {
            DialogResult result = MessageBox.Show(vMessage, AppGlobal.s_ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }

        /// <summary>
        /// 공용 메시지 박스입니다.
        /// </summary>
        public static DialogResult ShowMessage(System.Windows.Forms.IWin32Window vForm, string vMessage)
        {
            DialogResult result = ShowMessage(vForm, vMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }

        /// <summary>
        /// 기본 요청 정보를 생성합니다.
        /// </summary>
        /// <returns>생성된 요청 정보 객체 입니다.</returns>
        public static RequestCommunicationData MakeDefaultRequestData()
        {
            RequestCommunicationData tRequestData = new RequestCommunicationData();
            tRequestData.ClientID = AppGlobal.s_LoginResult.ClientID;
            return tRequestData;
        }

        /// <summary>
        /// 요청 데이터를 전송합니다.
        /// </summary>
        /// <param name="vSender">전송자 입니다.</param>
        /// <param name="vCommunicationData">전송 데이터 입니다.</param>
        public static void SendRequestData(ISenderObject vSender, CommunicationData vCommunicationData)
        {
            if (vSender != null)
            {
                AddSender(vSender);
                vCommunicationData.OwnerKey = vSender.GetHashCode();
            }
            lock (s_RequestQueue)
            {
                s_RequestQueue.Enqueue(vCommunicationData);
            }
            //2013-05-02 - shinyn - 요청시작시 ManualSet한다.
            //m_MRE.Set();
        }



        /// <summary>
        /// 요청 전송자를 추가 합니다.
        /// </summary>
        /// <param name="vSender">전송자 입니다.</param>
        public static void AddSender(ISenderObject vSender)
        {
            lock (s_SenderList)
            {
                if (!s_SenderList.ContainsKey(vSender.GetHashCode()))
                {
                    s_SenderList.Add(vSender.GetHashCode(), vSender);
                }
            }
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 매개변수와 일치하는 이름의 그룹정보가 있으면 반환합니다.
        /// </summary>
        /// <param name="aGroupName">찾고자 하는 그룹정보의 이름입니다.</param>
        /// <returns>일치하는 정보가 있으면 그룹정보를, 일치하는 정보가 없으면 null을 반환합니다.</returns>
        public static string getGroupID(string aGroupName)
        {
            foreach (GroupInfo tGroupInfo in s_GroupInfoList.InnerList.Values)
            {

                if (aGroupName.Equals(tGroupInfo.Name))
                {
                    return tGroupInfo.ID;
                }
            }

            return "-1";
        }


        private static TreeNodeEx SearchNode(TreeNodeCollection objNodes, string aParentID)
        {

            foreach (TreeNodeEx tNode in objNodes)
            {
                if (tNode.Tag.GetType().Equals(typeof(RACTCommonClass.GroupInfo)))
                {
                    GroupInfo tGroupInfo = (GroupInfo)tNode.Tag;

                    if (tGroupInfo.ID == aParentID) return tNode;

                    TreeNodeEx tFindNode = SearchNode(tNode.Nodes, aParentID);

                    if (tFindNode != null) return tFindNode;
                }
            }

            return null;

        }

        /// <summary>
        /// 2013-08-13- shinyn - 삭제할 그룹이 있으면 삭제하고 리스트업 한다.
        /// </summary>
        /// <param name="vTreeView"></param>
        /// <param name="aTreeType"></param>
        /// <param name="aDeleteGroupInfo"></param>
        public static void InitializeGroupTreeView(TreeViewEx vTreeView, E_TreeType aTreeType, GroupInfo aDeleteGroupInfo)
        {
            //TreeNodeEx tParentGroupNode = null;
            TreeNodeEx tGroupNode = null;
            TreeNodeEx tDeviceNode = null;
            TreeNodeEx tParentNode = null;

            // 2013-09-09 - shinyn- 사용자 그룹과 장비리스트를 로드
            if (aTreeType == E_TreeType.UserGroup || aTreeType == E_TreeType.UserGroupList)
            {
                if (AppGlobal.s_GroupInfoList == null) return;

                vTreeView.Nodes.Clear();
                int tDeviceCount = 0;
                try
                {
                    vTreeView.Visible = false;

                    // 2013-08-13 - 단계별 그룹리스트가 아닌 이전소스
                    /*
                    foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
                    {
                        tDeviceCount = AppGlobal.s_GroupInfoList.GetCountByGroup(tGroupInfo.ID);

                        tGroupNode = new TreeNodeEx(string.Concat(tGroupInfo.Name, "[", tDeviceCount, "]"), 0, 0);
                        tGroupNode.Tag = tGroupInfo;
                        //네트워크 실 노드 추가
                        vTreeView.Nodes.Add(tGroupNode);

                        foreach (DeviceInfo tDeviceInfo in tGroupInfo.DeviceList.InnerList)
                        {
                            tDeviceNode = new TreeNodeEx(string.Concat(tDeviceInfo.Name.Trim(), "[", tDeviceInfo.IPAddress, "]"), 1, 1);
                            tDeviceNode.Tag = tDeviceInfo;
                            tGroupNode.Nodes.Add(tDeviceNode);
                        }
                    }
                    */
                    // 2013-08-13 - 단계별 그룹리스트 표시



                    foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
                    {
                        //tDeviceCount = AppGlobal.s_GroupInfoList.GetCountByGroup(tGroupInfo.ID);
                        tDeviceCount = tGroupInfo.DEVICE_COUNT;

                        tGroupNode = new TreeNodeEx(string.Concat(tGroupInfo.Name, "[", tDeviceCount, "]"), 0, 0);
                        tGroupNode.Tag = tGroupInfo;


                        if (tGroupInfo.UP_ID != "")
                        {
                            tParentNode = SearchNode(vTreeView.Nodes, tGroupInfo.UP_ID);
                        }

                        if (tParentNode != null)
                        {
                            //네트워크 실 노드 추가
                            System.Diagnostics.Debug.WriteLine(tGroupInfo.Name + " 그룹 추가");
                            tParentNode.Nodes.Add(tGroupNode);
                        }
                        else
                        {
                            vTreeView.Nodes.Add(tGroupNode);
                        }

                        foreach (DeviceInfo tDeviceInfo in tGroupInfo.DeviceList.InnerList)
                        {
                            tDeviceNode = new TreeNodeEx(string.Concat(tDeviceInfo.Name.Trim(), "[", tDeviceInfo.IPAddress, "]"), 1, 1);
                            tDeviceNode.Tag = tDeviceInfo;
                            tGroupNode.Nodes.Add(tDeviceNode);
                        }
                    }

                    vTreeView.Visible = true;

                    // vTreeView.ExpandAll();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    vTreeView.Visible = true;

                }
            }
            else if (aTreeType == E_TreeType.SystemGroup)
            {

                if (AppGlobal.s_OrganizationInfo == null || AppGlobal.s_OrganizationInfo.AllGroupInfo == null) return;

                //이전 내용을 지우기 합니다.
                vTreeView.Nodes.Clear();
                Hashtable tGroupHash = null;
                int tDeviceCount = 0;

                try
                {
                    vTreeView.Visible = false;
                    tGroupNode = new TreeNodeEx("Root", 0, 0);
                    tGroupNode.Tag = AppGlobal.s_OrganizationInfo.AllGroupInfo;
                    //네트워크 실 노드 추가
                    vTreeView.Nodes.Add(tGroupNode);

                    if (AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups != null)
                    {
                        for (int i = 0; i < AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups.Count; i++)
                        {
                            AddGroupTree(tGroupNode, AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups[i]);
                        }
                    }
                    vTreeView.ExpandAll();
                    vTreeView.Visible = true;

                }
                catch (Exception ex)
                {
                    vTreeView.Visible = true;
                }
            }
            else if (aTreeType == E_TreeType.DisplayUserGroup)
            {
                if (AppGlobal.s_GroupInfoList == null) return;

                vTreeView.Nodes.Clear();
                int tDeviceCount = 0;
                try
                {
                    vTreeView.Visible = false;


                    tGroupNode = new TreeNodeEx("ROOT", 0, 0);
                    tGroupNode.Tag = new GroupInfo();

                    vTreeView.Nodes.Add(tGroupNode);

                    TreeNodeEx tTopNode = tGroupNode;

                    // 2013-08-13 - 단계별 그룹리스트 표시
                    foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
                    {
                        //tDeviceCount = AppGlobal.s_GroupInfoList.GetCountByGroup(tGroupInfo.ID);
                        tDeviceCount = tGroupInfo.DEVICE_COUNT;

                        tGroupNode = new TreeNodeEx(string.Concat(tGroupInfo.Name, "[", tDeviceCount, "]"), 0, 0);
                        tGroupNode.Tag = tGroupInfo;

                        // 보여지지 않을 그룹이 있는경우 보이지 않도록 처리한다.
                        if (aDeleteGroupInfo != null)
                        {
                            if (aDeleteGroupInfo.ID == tGroupInfo.ID) continue;
                        }

                        if (tGroupInfo.UP_ID != "")
                        {
                            tParentNode = SearchNode(tTopNode.Nodes, tGroupInfo.UP_ID);
                        }

                        if (tParentNode != null)
                        {
                            //네트워크 실 노드 추가
                            System.Diagnostics.Debug.WriteLine(tGroupInfo.Name + " 그룹 추가");
                            tParentNode.Nodes.Add(tGroupNode);
                        }
                        else
                        {
                            if (tGroupInfo.UP_ID == "" || tGroupInfo.UP_ID == "0")
                            {
                                tTopNode.Nodes.Add(tGroupNode);
                            }
                        }
                    }

                    vTreeView.Visible = true;

                    // vTreeView.ExpandAll();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    vTreeView.Visible = true;
                }
            }
            else if (aTreeType == E_TreeType.DisplayUserGroupList)
            {
                if (AppGlobal.s_UserInfoList == null) return;

                vTreeView.Nodes.Clear();
                int tDeviceCount = 0;
                try
                {
                    vTreeView.Visible = false;

                    // 2013-08-13 - 사용자에 대한 공유할 그룹리스트 표시
                    foreach (UserInfo tUserInfo in AppGlobal.s_UserInfoList)
                    {

                        GroupInfoCollection tGroupInfos = tUserInfo.GroupInfos;

                        TreeNodeEx tTopUserNode = new TreeNodeEx(string.Concat(tUserInfo.Name, "[", tUserInfo.Account, "]"), 0, 0);
                        tTopUserNode.Tag = tUserInfo;

                        vTreeView.Nodes.Add(tTopUserNode);

                    }
                    // vTreeView.ExpandAll();
                    vTreeView.Visible = true;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    vTreeView.Visible = true;

                }
            }


        }

        /// <summary>
        /// 맵트리를 초기화 합니다.
        /// </summary>
        /// <param name="vTV">맵트리를 초기화 할 트리뷰 입니다.</param>
        public static void InitializeGroupTreeView(TreeViewEx vTreeView, E_TreeType aTreeType)
        {
            InitializeGroupTreeView(vTreeView, aTreeType, null);
        }



        /// <summary>
        /// 지정한 노드의 하위에 장비 그룹을 추가합니다.
        /// </summary>
        /// <param name="vGroupHash">그룹 해시 테이블 입니다.</param>
        /// <param name="vParentNode">상위 노드 입니다.</param>
        /// <param name="vGroupInfo">그룹 정보 입니다.</param>
        private static void AddGroupTree(TreeNodeEx vParentNode, FACTGroupInfo vGroupInfo)
        {
            TreeNodeEx tGroupNode = null;
            int tImageIndex = 1;
            string tGroupName = "";
            string tGroupCode = "";

            if (vGroupInfo.CenterCode != "")
            {
                tImageIndex = 3;
                tGroupName = vGroupInfo.CenterName + "(" + vGroupInfo.GetToatalDeviceCount() + ")";
                tGroupCode = vGroupInfo.CenterCode;
            }
            else if (vGroupInfo.BranchCode != "")
            {
                tImageIndex = 2;
                tGroupName = vGroupInfo.BranchName + "(" + vGroupInfo.GetToatalDeviceCount() + ")";
                tGroupCode = vGroupInfo.BranchCode;
            }
            else
            {
                tImageIndex = 1;
                tGroupName = vGroupInfo.ORG1Name + "(" + vGroupInfo.GetToatalDeviceCount() + ")";
                tGroupCode = vGroupInfo.ORG1Code;
            }



            tGroupNode = new TreeNodeEx(tGroupName, 0, 0);

            tGroupNode.Tag = vGroupInfo;
            vParentNode.Nodes.Add(tGroupNode);

            if (vGroupInfo.SubGroups != null)
            {
                for (int i = 0; i < vGroupInfo.SubGroups.Count; i++)
                {
                    AddGroupTree(tGroupNode, vGroupInfo.SubGroups[i]);
                }
            }
        }

        /// <summary>
        /// 그리드 스타일을 초기화 합니다.
        /// </summary>
        /// <param name="vGrid">스타일을 초기화 할 그리드 입니다.</param>
        public static void InitializeGridStyle(C1FlexGrid vGrid)
        {
            vGrid.Styles["Normal"].BackColor = Color.FromArgb(255, 255, 255);
            vGrid.Styles["Normal"].ForeColor = Color.FromArgb(75, 75, 75); //그리드 안에 있는 폰트

            vGrid.Styles["Normal"].Border.Color = Color.FromArgb(225, 232, 242); //cell color		
            vGrid.Styles["Fixed"].BackColor = Color.FromArgb(245, 245, 245);
            vGrid.Styles["Fixed"].ForeColor = Color.FromArgb(0, 0, 0);
            vGrid.Styles["Fixed"].Border.Style = BorderStyleEnum.Flat;
            vGrid.Styles["Fixed"].Border.Color = Color.FromArgb(201, 201, 201);//임시로 처리함.
            vGrid.Styles["EmptyArea"].BackColor = Color.FromArgb(246, 246, 246); //데이터가 없을 경우 backcolor

            vGrid.Styles["Highlight"].ForeColor = Color.FromArgb(255, 255, 255);
            vGrid.Styles["Highlight"].BackColor = Color.FromArgb(110, 100, 189);
        }

        /// <summary>
        /// 콤보박스 스타일을 초기화 합니다
        /// </summary>
        /// <param name="cboDevicePart">콤보박스 입니다.</param>
        public static void InitializeComboBoxStyle(MKComboBox aComboBox)
        {
            aComboBox.BackColor = System.Drawing.SystemColors.Window;
            aComboBox.BackColorSelected = System.Drawing.Color.Orange;
            aComboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            aComboBox.BorderEdgeRadius = 3;
            aComboBox.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            aComboBox.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            aComboBox.BoxBorderColor = System.Drawing.Color.Gray;
            aComboBox.ColorBoxSize = new System.Drawing.Size(12, 8);
            aComboBox.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            aComboBox.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            aComboBox.ForeColor = System.Drawing.Color.Black;
            aComboBox.ImageList = null;
            aComboBox.ItemHeight = 14;
            aComboBox.MaxDorpDownWidth = 500;
            aComboBox.SelectedIndex = -1;
            aComboBox.ShowColorBox = false;
        }


        /// <summary>
        /// 장비 분류별 장비 모델 목록 입니다.
        /// </summary>
        public static Hashtable m_DeviceModelInfoDevicePart = new Hashtable();
        /// <summary>
        /// 장비 구분 목록을 초기화 합니다.
        /// </summary>
        public static void InitializeDevicePartList()
        {
            s_DevicePartList.Clear();
            m_DeviceModelInfoDevicePart.Clear();
            ArrayList tModelInfos = null;
            bool tIsMatch = false;

            foreach (ModelInfo tModelInfo in AppGlobal.s_ModelInfoList)
            {
                tIsMatch = false;
                for (int i = 0; i < s_DevicePartList.Count; i++)
                {
                    if (((DevicePartInfo)s_DevicePartList[i]).Name == tModelInfo.ModelTypeName)
                    {
                        tIsMatch = true;

                        if (((DevicePartInfo)s_DevicePartList[i]).IPTyep != 3 && ((DevicePartInfo)s_DevicePartList[i]).IPTyep != tModelInfo.IpTypeCd)
                        {
                            ((DevicePartInfo)s_DevicePartList[i]).IPTyep = 3;
                        }
                        break;
                    }
                }
                if (!tIsMatch)
                {
                    s_DevicePartList.Add(new DevicePartInfo(tModelInfo.ModelTypeName, tModelInfo.ViewOrder, tModelInfo.ModelTypeCode, tModelInfo.IpTypeCd));
                }
                //장비 분류별 모델 목록을 저장 합니다.
                tModelInfos = (ArrayList)m_DeviceModelInfoDevicePart[tModelInfo.ModelTypeName];
                if (tModelInfos == null)
                {
                    tModelInfos = new ArrayList();
                    m_DeviceModelInfoDevicePart.Add(tModelInfo.ModelTypeName, tModelInfos);
                }
                tModelInfos.Add(tModelInfo);
            }

            s_DevicePartList.Sort(new DevicePartCompare());

            foreach (ArrayList tModelList in m_DeviceModelInfoDevicePart.Values)
            {
                tModelList.Sort(new ModelSortCompare());
            }
        }


        /// <summary>
        /// 객체를 압축한 메모리스트림을 반환 합니다.
        /// </summary>
        /// <param name="aValue">압축할 객체 입니다.</param>
        /// <returns>메모리 스트림 입니다.</returns>
        public static object DecompressObject(CompressData aCompressData)
        {
            GZipStream tGZipStream = null;
            int tOffset = 0;
            int tBytesRead = 0;
            byte[] tBytes = new byte[aCompressData.OriginalSize + 100];

            aCompressData.Stream.Position = 0;
            tGZipStream = new GZipStream(aCompressData.Stream, CompressionMode.Decompress);
            while (true)
            {
                tBytesRead = tGZipStream.Read(tBytes, tOffset, 100);
                if (tBytesRead == 0) break;
                tOffset += tBytesRead;
            }
            object tObject = ObjectConverter.GetObject(tBytes);
            tGZipStream.Close();
            aCompressData.Dispose();
            return tObject;
        }

        public static void InitializeIPTypeComboBox(MKComboBox aIpTypeComboBox)
        {
            MKListItem tListItem = null;

            aIpTypeComboBox.Items.Clear();

            if (aIpTypeComboBox == null) return;

            foreach (E_IpType IpType in Enum.GetValues(typeof(E_IpType)))
            {
                if (IpType == E_IpType.ALL) continue;
                tListItem = aIpTypeComboBox.Items.Add(EnumUtil.GetDescription(IpType));
                tListItem.Tag = IpType;
            }
            /*
            foreach (E_IpType IpType in Enum.GetValues(typeof(E_IpType)))
            {
              aIpTypeComboBox.Items.Add(IpType.ToString());
            }
            */
        }

        /// <summary>
        /// 장비 분류 콤보박스 초기화 메서드
        /// </summary>
        /// <param name="aISManagementList">관리되는 장비분류만 표시할지의여부 </param>
        /// <param name="vcmbDivision">분류 콤보박스</param>
        public static void InitializeDevicePartComboBox(bool aISManagementList, MKComboBox vDevicePartComboBox)
        {
            InitializeDevicePartComboBox(aISManagementList, vDevicePartComboBox, true, null);
        }

        /// <summary>
        /// 장비 분류 콤보박스 초기화 메서드
        /// </summary>
        /// <param name="aISManagementList">관리되는 장비분류만 표시할지의여부 </param>
        /// <param name="vcmbDivision">분류 콤보박스</param>
        /// <param name="aAllType">전체 장비분류인지의 여부</param>
        /// <param name="aSelectType">선택된 장비분류 리스트(,로 분리)</param>
        public static void InitializeDevicePartComboBox(bool aISManagementList, MKComboBox vDevicePartComboBox, bool aAllType, ArrayList aSelectTypeList)
        {
            MKListItem tListItem = null;
            DevicePartInfo[] tItems = null;
            string[] tConditions = null;



            tItems = new DevicePartInfo[AppGlobal.s_DevicePartList.Count];
            tConditions = new string[AppGlobal.s_DevicePartList.Count];
            for (int i = 0; i < AppGlobal.s_DevicePartList.Count; i++)
            {
                tConditions[i] = ((DevicePartInfo)AppGlobal.s_DevicePartList[i]).Name;
                tItems[i] = (DevicePartInfo)AppGlobal.s_DevicePartList[i];
            }

            // 알파벳 순으로 항목을 정렬.
            Array.Sort(tConditions, tItems);

            vDevicePartComboBox.Items.Clear();
            vDevicePartComboBox.Items.Add("- 분류 선택 -");
            /*
            for (int i = 0; i < tItems.Length; i++)
            {
                tListItem = vDevicePartComboBox.Items.Add(((DevicePartInfo)tItems[i]).Name);
                tListItem.Tag = tItems[i];
            }
            */
            for (int i = 0; i < tItems.Length; i++)
            {
                if (AppGlobal.m_ViewIPType == E_IpType.IPTV
                             && (((DevicePartInfo)tItems[i]).IPTyep == 1
                             || ((DevicePartInfo)tItems[i]).IPTyep == 3))
                {
                    tListItem = vDevicePartComboBox.Items.Add(((DevicePartInfo)tItems[i]).Name);
                    tListItem.Tag = tItems[i];
                }
                else
                 if (AppGlobal.m_ViewIPType == E_IpType.CATV
                             && (((DevicePartInfo)tItems[i]).IPTyep == 2
                             || ((DevicePartInfo)tItems[i]).IPTyep == 3))
                {
                    tListItem = vDevicePartComboBox.Items.Add(((DevicePartInfo)tItems[i]).Name);
                    tListItem.Tag = tItems[i];
                }
            }
        }

        /// <summary>
        /// 장비 모델 콤보 박스를 초기화 합니다.
        /// </summary>
        /// <param name="vDeviceModelComboBox">장비 모델 콤보 박스 입니다.</param>
        /// <param name="vDevicePart">장비 분류 입니다.</param>
        public static void InitializeDeviceModelComboBox(MKComboBox vDeviceModelComboBox, string vDevicePart)
        {
            MKListItem tListItem = null;
            vDeviceModelComboBox.Enabled = false;
            vDeviceModelComboBox.Items.Clear();
            vDeviceModelComboBox.Items.Add("- 모델 선택 -");

            if (m_DeviceModelInfoDevicePart[vDevicePart] == null) return;

            ModelInfo[] tItems = new ModelInfo[((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart]).Count];

            string[] tConditions = new string[((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart]).Count];
            for (int i = 0; i < ((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart]).Count; i++)
            {
                tConditions[i] = ((ModelInfo)((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart])[i]).ModelName;
                tItems[i] = (ModelInfo)((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart])[i];
            }

            // 알파벳 순으로 항목을 정렬.
            Array.Sort(tConditions, tItems);

            int tIPTypeCd = (m_ViewIPType == E_IpType.IPTV) ? 1 : (m_ViewIPType == E_IpType.CATV) ? 2 : 3;

            for (int i = 0; i < tItems.Length; i++)
            {
                if (((ModelInfo)tItems[i]).IpTypeCd == tIPTypeCd)
                {
                    tListItem = vDeviceModelComboBox.Items.Add(((ModelInfo)tItems[i]).ModelName);
                    tListItem.Tag = tItems[i];
                }
            }
            //2008-05-01 hanjiyeon 수정 - 모든 드롭다운리스트의 아이템을 정렬시켜 표시 - end
            vDeviceModelComboBox.Enabled = true;
        }

        /// <summary>
        /// 네트워크 실 코드를 이용하여 네트워크 실 이름을 가져오기 합니다.
        /// </summary>
        /// <param name="aORG1Code"></param>
        /// <returns></returns>
        public static string GetORG1Name(string aORG1Code)
        {
            if (AppGlobal.s_OrganizationInfo.ORG1NameByCode[(string)aORG1Code] != null)
            {
                return (string)AppGlobal.s_OrganizationInfo.ORG1NameByCode[(string)aORG1Code];
            }

            return "";
        }

        /// <summary>
        /// 팀 코드를 이용하여 팀 명을 가져오기 합니다.
        /// </summary>
        /// <param name="aIsBranchCode">전달인자가 지사코드인지의 여부입니다.false:센터코드</param>
        /// <param name="vBranchCode">조직코그 입니다.</param>
        /// <returns></returns>
        public static string GetBranchName(string vBranchCode)
        {
            if (AppGlobal.s_OrganizationInfo.BranchNameByCode[(string)vBranchCode] != null)
            {
                return (string)AppGlobal.s_OrganizationInfo.BranchNameByCode[(string)vBranchCode];
            }

            return "";
        }

        /// <summary>
        /// 센터 코드명을 이용하여 센터명을 가져오기 합니다.
        /// </summary>
        /// <param name="vCenterCode"></param>
        /// <returns></returns>
        public static string GetCenterName(string vCenterCode)
        {
            if (AppGlobal.s_OrganizationInfo.CenterNameByCode[(string)vCenterCode] != null)
            {
                return (string)AppGlobal.s_OrganizationInfo.CenterNameByCode[(string)vCenterCode];
            }

            return "";
        }

        /// <summary>
        /// 센터 코드를 이용하여 조직정보(만)를 가져옵니다. 장비정보는 없음.
        /// </summary>
        /// <returns></returns>
        public static FACTGroupInfo GetGroupInfoByCenterCode(string aCenterCode)
        {
            FACTGroupInfo tGroupInfo = null;

            if (AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups.Count > 0)
            {
                for (int i = 0; i < AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups.Count; i++)
                {
                    //네트워크 실
                    for (int t = 0; t < AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups[i].SubGroups.Count; t++)
                    {
                        for (int c = 0; c < AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups[i].SubGroups[t].SubGroups.Count; c++)
                        {
                            if (AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups[i].SubGroups[t].SubGroups[c].CenterCode == aCenterCode)
                            {
                                tGroupInfo = (FACTGroupInfo)AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups[i].SubGroups[t].SubGroups[c];
                                return tGroupInfo;
                            }
                        }
                    }
                }
            }

            return tGroupInfo;
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

            Image tImage = global::RACTClient.Properties.Resources.nomal_center;
            ilCenter.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.hover_center;
            ilCenter.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.click_center;
            ilCenter.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.disable_center;
            ilCenter.Images.Add(tImage);

            tImage = global::RACTClient.Properties.Resources.nomal_left;
            ilLeft.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.hover_left;
            ilLeft.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.click_left;
            ilLeft.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.disable_left;
            ilLeft.Images.Add(tImage);

            tImage = global::RACTClient.Properties.Resources.nomal_right;
            ilRight.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.hover_right;
            ilRight.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.click_right;
            ilRight.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.disable_center;
            ilRight.Images.Add(tImage);
        }

        /// <summary>
        /// XML 파일을 생성 합니다.
        /// </summary>
        public static void MakeClientOption()
        {
            // 2013-04-24- shinyn - 환경설정파일 저장시 오류체크
            try
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    AppGlobal.s_ClientOption.ServerIP = AppGlobal.s_ServerIP;
                }
                MKXML.ObjectToXML(Application.StartupPath + "\\ClientOption.xml", AppGlobal.s_ClientOption);
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog("MakeClientOption : " + ex.Message);
            }


        }

        /// <summary>
        /// 스크립트 파일을 열기합니다.
        /// </summary>
        /// <returns></returns>
        public static DialogResult ShowScriptOpenDialog(out string aScriptText)
        {
            aScriptText = "";

            OpenFileDialog tOpenDialog = new OpenFileDialog();
            tOpenDialog.DefaultExt = "tacts";
            tOpenDialog.Filter = "TACT Script Files (*.tacts)|*.tacts|All Files (*.*)|*.*";

            if (tOpenDialog.ShowDialog(AppGlobal.s_ClientMainForm) != DialogResult.OK) return DialogResult.Cancel;
            aScriptText = File.ReadAllText(tOpenDialog.FileName);
            return DialogResult.OK;
        }

        /// <summary>
        /// 프로그래스를 표시를 처리 합니다.
        /// </summary>
        /// <param name="aVisable"></param>
        public static void ShowLoadingProgress(bool aVisable)
        {
            ((ClientMain)AppGlobal.s_ClientMainForm).ShowLoadingProgress(aVisable);
        }

        /// <summary>
        /// 2013-01-11 - shinyn - R14-01 - 장비 목록 저장 
        /// </summary>
        /// <param name="aDeviceInfos"></param>
        /// <returns></returns>
        public static bool SaveDeviceList(UserInfo aUserInfo, DeviceInfoCollection aDeviceInfos, DeviceCfgSaveInfoCollection aDviceCfgSaveInfos)
        {
            /*   
             -- XML형식
            <?xml version="1.0" ?> 
            <Account></Account>
            <DeviceInfos>
              <DeviceIDs>
                  <DeviceID value="211987">
    	                <Name>장비이름</Name>
                        <ModelID>모델아이디</ModelID>
			            <BranchCode></BranchCode>
			            <CenterCode></CenterCode>
			            <TpoName></TpoName>
			            <IPAddress></IPAddress>
			            <DevicePartCode></DevicePartCode>
			            <InputFlag></InputFlag>
                        <Version></Version>
                        <DeviceNumber></DeviceNumber>
                        <GroupID></GroupID>
                        <ApplyDate></ApplyDate>
                        <DeviceGroupName></DeviceGroupName>
                        <Description></Description>
                        <ORG1Code></ORG1Code>
                        <ORG1Name></ORG1Name>
                        <ORG2Code></ORG2Code>
                        <ORG2Name></ORG2Name>
                        <CenterName></CenterName>
                        <ModelName></ModelName>
                        <CfgSaveInfos>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                        <CfgSaveInfos>
                  </DeviceID> 
                  <DeviceID value="211987">
    	                <Name>장비이름</Name>
                        <ModelID>모델아이디</ModelID>
			            <BranchCode></BranchCode>
			            <CenterCode></CenterCode>
			            <TpoName></TpoName>
			            <IPAddress></IPAddress>
			            <DevicePartCode></DevicePartCode>
			            <InputFlag></InputFlag>
                        <Version></Version>
                        <DeviceNumber></DeviceNumber>
                        <GroupID></GroupID>
                        <ApplyDate></ApplyDate>
                        <DeviceGroupName></DeviceGroupName>
                        <Description></Description>
                        <ORG1Code></ORG1Code>
                        <ORG1Name></ORG1Name>
                        <ORG2Code></ORG2Code>
                        <ORG2Name></ORG2Name>
                        <CenterName></CenterName>
                        <ModelName></ModelName>
                        <CfgSaveInfos>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                        <CfgSaveInfos>
                  </DeviceID> 
              </DeviceIDs>
            </DeviceInfos>	
            */

            SaveFileDialog tSaveFileDialog = new SaveFileDialog();

            tSaveFileDialog.Filter = "XML Files(*.xml)|*.xml";

            DateTime tNow = DateTime.Now;

            string tFileName = "[" +
                               tNow.Year.ToString("0000") + "-" +
                               tNow.Month.ToString("00") + "-" +
                               tNow.Day.ToString("00") + "_" +
                               tNow.Hour.ToString("00") +
                               tNow.Minute.ToString("00") +
                               tNow.Second.ToString("00") + "]" +
                               "장비목록.XML";


            tSaveFileDialog.FileName = tFileName;


            if (tSaveFileDialog.ShowDialog() != DialogResult.OK) return false;

            string tSaveFilePath = tSaveFileDialog.FileName;

            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);


            XmlElement rootNode = doc.CreateElement("DeviceInfos");
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
            doc.AppendChild(rootNode);

            XmlElement tAccountNode = doc.CreateElement("Account");
            rootNode.AppendChild(tAccountNode);
            tAccountNode.InnerText = aUserInfo.Account.ToString();

            XmlElement tDeviceIDsNode = doc.CreateElement("DeviceIDs");
            rootNode.AppendChild(tDeviceIDsNode);

            int tDevCount = 0;


            int tIndex = 0;

            foreach (DeviceInfo tDeviceInfo in aDeviceInfos)
            {
                XmlElement xmlDeviceID = doc.CreateElement("DeviceID");
                xmlDeviceID.SetAttribute("value", tDeviceInfo.DeviceID.ToString());

                tDeviceIDsNode.AppendChild(xmlDeviceID);

                XmlElement xmlChild = doc.CreateElement("Name");
                xmlChild.InnerText = tDeviceInfo.Name.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ModelID");
                xmlChild.InnerText = tDeviceInfo.ModelID.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("BrnachCode");
                xmlChild.InnerText = tDeviceInfo.BranchCode.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("TpoName");
                xmlChild.InnerText = tDeviceInfo.TpoName.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("IPAddress");
                xmlChild.InnerText = tDeviceInfo.IPAddress.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("DevicePartCode");
                xmlChild.InnerText = tDeviceInfo.DevicePartCode.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("InputFlag");
                xmlChild.InnerText = Convert.ToString((int)tDeviceInfo.InputFlag);
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("Version");
                xmlChild.InnerText = tDeviceInfo.Version.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("DeviceNumber");
                xmlChild.InnerText = tDeviceInfo.DeviceNumber.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("GroupID");
                xmlChild.InnerText = tDeviceInfo.GroupID.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ApplyDate");
                xmlChild.InnerText = tDeviceInfo.ApplyDate.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("DeviceGroupName");
                xmlChild.InnerText = tDeviceInfo.DeviceGroupName.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("Description");
                xmlChild.InnerText = tDeviceInfo.Description.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ORG1Code");
                xmlChild.InnerText = tDeviceInfo.ORG1Code.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ORG1Name");
                xmlChild.InnerText = tDeviceInfo.ORG1Name.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ORG2Code");
                xmlChild.InnerText = tDeviceInfo.ORG2Code.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ORG2Name");
                xmlChild.InnerText = tDeviceInfo.ORG2Name.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("CenterName");
                xmlChild.InnerText = tDeviceInfo.CenterName.ToString();
                xmlDeviceID.AppendChild(xmlChild);

                xmlChild = doc.CreateElement("ModelName");
                xmlChild.InnerText = AppGlobal.s_ModelInfoList[tDeviceInfo.ModelID].ModelName;
                xmlDeviceID.AppendChild(xmlChild);

                DeviceCfgSaveInfo tDeviceCfgSaveInfo = (DeviceCfgSaveInfo)aDviceCfgSaveInfos.InnerList[tIndex];


                XmlElement xmlCfgSaveInfos = doc.CreateElement("CfgSaveInfos");
                xmlDeviceID.AppendChild(xmlCfgSaveInfos);

                foreach (CfgSaveInfo tCfgSaveInfo in tDeviceCfgSaveInfo.CfgSaveInfoCollection)
                {
                    XmlElement xmlCfgSaveInfo = doc.CreateElement("CfgSaveInfo");
                    xmlCfgSaveInfo.SetAttribute("value", tCfgSaveInfo.Iden.ToString());
                    xmlCfgSaveInfos.AppendChild(xmlCfgSaveInfo);

                    XmlElement xmlCfgChild = doc.CreateElement("StTime");
                    if (tCfgSaveInfo.StTime != null)
                    {
                        xmlCfgChild.InnerText = tCfgSaveInfo.StTime.ToShortDateString() + " " +
                                                tCfgSaveInfo.StTime.ToShortTimeString();
                    }
                    xmlCfgSaveInfo.AppendChild(xmlCfgChild);

                    xmlCfgChild = doc.CreateElement("FullFileName");

                    string tFullFileName = tCfgSaveInfo.FileName;
                    if (tCfgSaveInfo.FileExtend != "")
                    {
                        tFullFileName = tFullFileName + "." + tCfgSaveInfo.FileExtend;
                    }
                    xmlCfgChild.InnerText = tFullFileName;
                    xmlCfgSaveInfo.AppendChild(xmlCfgChild);

                    xmlCfgChild = doc.CreateElement("RestoreCommandScript");
                    string tScript = GetScript(tCfgSaveInfo.CfgRestoreCommands);
                    xmlCfgChild.InnerText = tScript;
                    xmlCfgSaveInfo.AppendChild(xmlCfgChild);
                }

                tIndex++;

            }

            XmlTextWriter tWriter = new XmlTextWriter(tSaveFilePath, null);
            tWriter.Formatting = Formatting.Indented;
            doc.Save(tWriter);

            tWriter.Close();

            return true;


        }

        /// <summary>
        /// 2013-01-11 - shinyn - CFG 복원명령을 Script 형태로 반환한다.
        /// </summary>
        /// <param name="aCfgRestoreCommands"></param>
        /// <returns></returns>
        public static string GetScript(CfgRestoreCommandCollection aCfgRestoreCommands)
        {
            StringBuilder tRet = new StringBuilder();

            tRet.Append("Sub Main \r\n");

            foreach (CfgRestoreCommand tCommand in aCfgRestoreCommands)
            {
                if (tCommand != null)
                {
                    tRet.Append("\t" + Script.s_Send + " \"" + tCommand.Cmd + "\"&chr(13)\r\n");
                    tRet.Append("\t" + Script.s_WaitForString + " \"" + tCommand.T_Prompt + "\"\r\n");
                }
            }

            tRet.Append("End Sub");

            return tRet.ToString();

        }


        /// <summary>
        /// 2013-05-02 - shinyn - 링크장비연결에 필요한 Telnet명령 스크립트를 만든다.
        /// </summary>
        /// <param name="aCfgRestoreCommands"></param>
        /// <returns></returns>
        public static string GetTelnetScript(DeviceInfo aDeviceInfo, FACT_DefaultConnectionCommandSet aCommandSet)
        {
            StringBuilder tRet = new StringBuilder();

            tRet.Append("Sub Main \r\n");

            // 0 : ${WAIT}       default : n:|:
            // 1 : ${USERID1}    default : d: 
            // 2 : ${PASSWORD1}  default : #|>
            // 3 : ${USERID2}    default : d:|#
            // 4 : ${PASSWORD2}  default : #
            for (int i = 0; i < aCommandSet.CommandList.Count; i++)
            {
                FACT_DefaultConnectionCommand tCommand = aCommandSet.CommandList[i];

                //0 : ${WAIT}
                if (i == 0)
                {
                    tRet.Append("\t" + Script.s_Send + " \"telnet " + aDeviceInfo.IPAddress + "\"&chr(13)\r\n");
                    tRet.Append("\t" + Script.s_WaitForString + " \"" + tCommand.Prompt + "\"\r\n");
                }

                //1 : ${USERID1} 
                if (i == 1)
                {
                    tRet.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetID1 + "\"&chr(13)\r\n");
                    tRet.Append("\t" + Script.s_WaitForString + " \"" + tCommand.Prompt + "\"\r\n");
                }

                //2 : ${PASSWORD1} 
                if (i == 2)
                {
                    tRet.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetPwd1 + "\"&chr(13)\r\n");
                    tRet.Append("\t" + Script.s_WaitForString + " \"" + tCommand.Prompt + "\"\r\n");
                }

                //3 : ${USERID2}
                if (i == 3)
                {
                    tRet.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetID2 + "\"&chr(13)\r\n");
                    tRet.Append("\t" + Script.s_WaitForString + " \"" + tCommand.Prompt + "\"\r\n");
                }

                //4 : ${PASSWORD2}
                if (i == 4)
                {
                    tRet.Append("\t" + Script.s_Send + " \"" + aDeviceInfo.TelnetPwd2 + "\"&chr(13)\r\n");
                    tRet.Append("\t" + Script.s_WaitForString + " \"" + tCommand.Prompt + "\"\r\n");
                }

            }
            tRet.Append("End Sub");

            return tRet.ToString();

        }

        public static DeviceInfoCollection OpenDeviceList()
        {
            /*   
             -- XML형식
            <?xml version="1.0" ?> 
            <Account></Account>
            <DeviceInfos>
              <DeviceIDs>
                  <DeviceID value="211987">
    	                <Name>장비이름</Name>
                        <ModelID>모델아이디</ModelID>
			            <BranchCode></BranchCode>
			            <CenterCode></CenterCode>
			            <TpoName></TpoName>
			            <IPAddress></IPAddress>
			            <DevicePartCode></DevicePartCode>
			            <InputFlag></InputFlag>
                        <Version></Version>
                        <DeviceNumber></DeviceNumber>
                        <GroupID></GroupID>
                        <ApplyDate></ApplyDate>
                        <DeviceGroupName></DeviceGroupName>
                        <Description></Description>
                        <ORG1Code></ORG1Code>
                        <ORG1Name></ORG1Name>
                        <ORG2Code></ORG2Code>
                        <ORG2Name></ORG2Name>
                        <CenterName></CenterName>
                        <ModelName></ModelName>
                        <CfgSaveInfos>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                        <CfgSaveInfos>
                  </DeviceID> 
                  <DeviceID value="211987">
    	                <Name>장비이름</Name>
                        <ModelID>모델아이디</ModelID>
			            <BranchCode></BranchCode>
			            <CenterCode></CenterCode>
			            <TpoName></TpoName>
			            <IPAddress></IPAddress>
			            <DevicePartCode></DevicePartCode>
			            <InputFlag></InputFlag>
                        <Version></Version>
                        <DeviceNumber></DeviceNumber>
                        <GroupID></GroupID>
                        <ApplyDate></ApplyDate>
                        <DeviceGroupName></DeviceGroupName>
                        <Description></Description>
                        <ORG1Code></ORG1Code>
                        <ORG1Name></ORG1Name>
                        <ORG2Code></ORG2Code>
                        <ORG2Name></ORG2Name>
                        <CenterName></CenterName>
                        <ModelName></ModelName>
                        <CfgSaveInfos>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                            <CfgSaveInfo Value="">
                                <StTime></StTime>
                                <FullFileName></FullFileName>
                                <RestoreCommandScript></RestoreCommandScript>
                            </CfgSaveInfo>
                        <CfgSaveInfos>
                  </DeviceID> 
              </DeviceIDs>
            </DeviceInfos>	
            */

            OpenFileDialog tOpenFileDialog = new OpenFileDialog();

            tOpenFileDialog.Filter = "XML Files(*.xml)|*.xml";

            DialogResult tDialogResult = tOpenFileDialog.ShowDialog();

            if (tDialogResult != DialogResult.OK) return null;

            string tFilePath = tOpenFileDialog.FileName;

            XmlDocument doc = new XmlDocument();

            doc.Load(tFilePath);

            XmlNodeList xmlList = doc.SelectNodes("DeviceInfos/DeviceIDs/DeviceID");

            int i = 0;

            DeviceInfoCollection tDeviceInfos = new DeviceInfoCollection();

            for (i = 0; i < xmlList.Count; i++)
            {
                DeviceInfo tDeviceInfo = new DeviceInfo();

                tDeviceInfo.DeviceID = Convert.ToInt32(xmlList[i].Attributes["value"].Value);

                XmlNodeList tChild = xmlList[i].ChildNodes;

                tDeviceInfo.Name = xmlList[i].ChildNodes[0].InnerText.ToString();
                tDeviceInfo.BranchCode = xmlList[i].ChildNodes[1].InnerText.ToString();
                tDeviceInfo.CenterCode = xmlList[i].ChildNodes[2].InnerText.ToString();
                tDeviceInfo.TpoName = xmlList[i].ChildNodes[3].InnerText.ToString();
                tDeviceInfo.IPAddress = xmlList[i].ChildNodes[4].InnerText.ToString();
                tDeviceInfo.DevicePartCode = Convert.ToInt32(xmlList[i].ChildNodes[5].InnerText);
                tDeviceInfo.InputFlag = (E_FlagType)Convert.ToInt32(xmlList[i].ChildNodes[6].InnerText);
                tDeviceInfo.Version = xmlList[i].ChildNodes[7].InnerText;
                tDeviceInfo.DeviceNumber = xmlList[i].ChildNodes[8].InnerText;
                tDeviceInfo.GroupID = xmlList[i].ChildNodes[9].InnerText;
                tDeviceInfo.ApplyDate = Convert.ToDateTime(xmlList[i].ChildNodes[10].InnerText);
                tDeviceInfo.DeviceGroupName = xmlList[i].ChildNodes[11].InnerText;
                tDeviceInfo.Description = xmlList[i].ChildNodes[12].InnerText;
                tDeviceInfo.ORG1Code = xmlList[i].ChildNodes[13].InnerText;
                tDeviceInfo.ORG1Name = xmlList[i].ChildNodes[14].InnerText;
                tDeviceInfo.ORG2Code = xmlList[i].ChildNodes[15].InnerText;
                tDeviceInfo.ORG2Name = xmlList[i].ChildNodes[16].InnerText;
                tDeviceInfo.CenterName = xmlList[i].ChildNodes[17].InnerText;
                tDeviceInfo.ModelName = xmlList[i].ChildNodes[18].InnerText;


                XmlNodeList xmlCfgSaveInfos = xmlList[i].SelectNodes("CfgSaveInfos/CfgSaveInfo");

                for (int j = 0; j < xmlCfgSaveInfos.Count; j++)
                {
                    CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();

                    if (xmlCfgSaveInfos[j].ChildNodes[0].InnerText != "")
                    {
                        tCfgSaveInfo.StTime = Convert.ToDateTime(xmlCfgSaveInfos[j].ChildNodes[0].InnerText);
                    }

                    tCfgSaveInfo.FullFileName = xmlCfgSaveInfos[j].ChildNodes[1].InnerText;
                    tCfgSaveInfo.CfgRestoreScript = xmlCfgSaveInfos[j].ChildNodes[2].InnerText;

                    tDeviceInfo.CfgSaveInfos.Add(tCfgSaveInfo);
                }


                tDeviceInfos.Add(tDeviceInfo);
            }

            return tDeviceInfos;
        }


        /// <summary>
        /// 2013-04-19 - shinyn - Grid복사 붙여넣기 할경우 사용함
        /// </summary>
        /// <param name="vGrid"></param>
        /// <param name="isControl"></param>
        /// <param name="vKey"></param>
        /// <param name="isNoPaste"></param>
        /// <returns></returns>
        public static E_ClipboardProcessType GridClipBoard(C1.Win.C1FlexGrid.C1FlexGrid vGrid, bool isControl, Keys vKey, bool isNoPaste)
        {
            return GridClipBoard(vGrid, isControl, vKey, isNoPaste, false);
        }
        /// <summary>
        /// 2013-04-19 - shinyn - Grid복사 붙여넣기 할경우 사용함
        /// </summary>
        /// <param name="vGrid"></param>
        /// <param name="isControl"></param>
        /// <param name="vKey"></param>
        /// <param name="isNoPaste"></param>
        /// <param name="isAllGridCopy"></param>
        /// <returns></returns>
        public static E_ClipboardProcessType GridClipBoard(C1.Win.C1FlexGrid.C1FlexGrid vGrid, bool isControl, Keys vKey, bool isNoPaste, bool isAllGridCopy)
        {
            E_ClipboardProcessType tProcessType = E_ClipboardProcessType.None;

            if (!isControl) return tProcessType;

            try
            {
                switch (vKey)
                {
                    case Keys.C:
                        string tClipString = "";
                        for (int i = 1; i < vGrid.Rows.Count; i++)
                        {
                            if (vGrid.Rows[i].Selected || isAllGridCopy)
                            {
                                for (int tCol = vGrid.Cols.Fixed; tCol < vGrid.Cols.Count; tCol++)
                                {
                                    if (vGrid[i, tCol] != null)
                                    {
                                        tClipString += vGrid[i, tCol].ToString();
                                    }
                                    tClipString += "\t";
                                }
                                tClipString += "\r\n";
                            }
                        }
                        if (tClipString != "")
                        {
                            Clipboard.SetDataObject(tClipString);
                            tProcessType = E_ClipboardProcessType.Copy;
                        }
                        break;

                    case Keys.V:
                        if (!isNoPaste)
                        {
                            IDataObject tData = Clipboard.GetDataObject();
                            if (tData.GetDataPresent(typeof(string)))
                            {
                                string[] tStringLine = ((string)tData.GetData(typeof(string))).Split('\n');
                                string[] tValues = null;
                                int tRow = vGrid.Row;
                                //mjjoe, 2008.10.21 코드추가 ---------------------------------------------------------------------------------
                                int tAddNewOption_AddLine;
                                if (vGrid.AllowAddNew == true)
                                    tAddNewOption_AddLine = 2;
                                else
                                    tAddNewOption_AddLine = 1;

                                Row tAddedRow;
                                //mjjoe, 2008.10.21 코드추가 끝---------------------------------------------------------------------------------
                                for (int i = 0; i < tStringLine.Length; i++)
                                {
                                    //mjjoe, 2008.10.21 코드추가 ---------------------------------------------------------------------------------
                                    //클립보드 복사시 마지막에 빈행이 더해져서 복사되어 진다.
                                    //(Excel에서 테스트). 그러므로, 마지막 행이 빈 행이면 Line을 추가하지 않도록 처리한다.
                                    if (i == tStringLine.Length - 1 && tStringLine[i] == "")
                                        break;
                                    //mjjoe, 2008.10.21 코드추가 끝 ---------------------------------------------------------------------------------

                                    //mjjoe, 2008.10.21 수정 ---------------------------------------------------------------------------------
                                    //if(tRow >= vGrid.Rows.Count) //old코드
                                    //{ 
                                    //    vGrid.Rows.Count++;
                                    //}
                                    if (tRow > vGrid.Rows.Count - tAddNewOption_AddLine)
                                    {
                                        tAddedRow = vGrid.Rows.Add();
                                    }
                                    else
                                    {
                                        tAddedRow = vGrid.Rows[tRow];
                                    }

                                    tValues = tStringLine[i].Split('\t');

                                    for (int tCol = 0; tCol < tValues.Length; tCol++)
                                    {
                                        if (vGrid.Col + tCol >= vGrid.Cols.Count) break;

                                        //vGrid[tRow, tCol + vGrid.Col] = tValues[tCol].Replace(((char)13).ToString(), ""); //old코드
                                        tAddedRow[tCol + vGrid.Col] = tValues[tCol].Replace(((char)13).ToString(), "");
                                    }
                                    //mjjoe, 2008.10.21 수정 끝---------------------------------------------------------------------------------

                                    tRow++;
                                }
                                tProcessType = E_ClipboardProcessType.Paste;
                            }
                        }
                        break;

                    case Keys.A:
                        vGrid.Select(1, vGrid.Cols.Fixed, vGrid.Rows.Count - 1, vGrid.Cols.Count - 1);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return tProcessType;
        }
        public static E_ClipboardProcessType GridClipBoardEx(C1.Win.C1FlexGrid.C1FlexGrid vGrid, bool isControl, Keys vKey, bool isNoPaste, bool isAllGridCopy)
        {
            return E_ClipboardProcessType.Copy;
        }

        //2015-10-28 hanjiyeon 추가
        /// <summary>
        /// 알카텔루슨트 장비인지의 여부를 반환합니다.
        /// </summary>
        /// <param name="aModelID"></param>
        /// <returns></returns>
        public static bool IsAlLuDevice(int aModelID)
        {
            if (aModelID == 4035 ||
                aModelID == 4036)
            {
                return true;
            }

            return false;
        }

        //2018-11-20 KangBonghan 추가
        /// <summary>
        /// RPCS 장비인지의 여부를 반환합니다.
        /// </summary>
        /// <param name="aModelID"></param>
        /// <returns></returns>
        public static bool IsRpcsDevice(int aModelID)
        {
            if (aModelID == 4150 || aModelID == 4312)
            {
                return true;
            }

            return false;
        }

        public static bool IsRpcsModel(int aModelType)
        {
            if (aModelType == 34)
            {
                return true;
            }

            return false;
        }

        // public static bool bPanelFocusCheck = false;

        public static TerminalPanel terminalPanel1;

        public static bool IsValidIPv4(string ip, out string returnip)
        {
            returnip = "";

            if (string.IsNullOrWhiteSpace(ip))
                return false;

            string[] rawParts = ip.Split('.');
            List<string> segments = new List<string>();
            int validPartCount = 0; // 유효한 옥텟 개수

            for (int i = 0; i < rawParts.Length; i++)
            {
                string part = rawParts[i];

                // 중간에 빈 값이 있으면 실패 (예: 210..10.5)
                if (string.IsNullOrWhiteSpace(part))
                {
                    if (i != rawParts.Length - 1)
                        return false;

                    part = "0"; // 마지막 빈 옥텟은 0으로 보충
                }
                else
                {
                    validPartCount++; // 유효한 값이면 카운트
                }

                segments.Add(part);
            }

            // ★ 수정된 조건: 유효한 옥텟 개수가 3개 이상이어야 함
            if (validPartCount < 3)
                return false;

            // 최대 4옥텟까지 0으로 보충
            while (segments.Count < 4)
                segments.Add("0");

            if (segments.Count > 4)
                return false;

            // 숫자 및 범위 검사
            for (int i = 0; i < 4; i++)
            {
                string seg = segments[i];

                if (!seg.All(Char.IsDigit))
                    return false;

                int val;
                if (!int.TryParse(seg, out val) || val < 0 || val > 255)
                    return false;

                segments[i] = val.ToString(); // 앞자리 0 제거
            }

            returnip = string.Join(".", segments);
            return true;
        }

    }
}
