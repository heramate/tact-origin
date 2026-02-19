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
    /// 2013-04-19 - shinyn - �׸��忡�� ���� �ٿ��ֱ�� ����մϴ�.
    /// </summary>
    public enum E_ClipboardProcessType
    {
        /// <summary>
        /// ó�� ���� �Դϴ�.
        /// </summary>
        None,
        /// <summary>
        /// ���� ó�� �Դϴ�.
        /// </summary>
        Copy,
        /// <summary>
        /// �ٿ��ֱ� ó�� �Դϴ�.
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
        /// �������� ���� ��� �Դϴ�.
        /// </summary>
        public static E_RACTClientMode s_RACTClientMode = E_RACTClientMode.Online;
        /// <summary>
        /// �������� �ý����� ȣ���� �ý��� Ÿ�� �Դϴ�.
        /// </summary>
        public static E_TerminalMode s_Caller = E_TerminalMode.RACTClient;
        /// <summary>
        /// ������������ �����ϱ� ���� ����� ����
        /// </summary>
        public static bool m_DirectConnect = false;

        public static string[] s_FontList = new string[] {"����ü"
                                                        ,"����ü"
                                                        ,"�ü�ü"
                                                        ,"����ü"
                                                        ,"Arial"
                                                        ,"Calibri"
                                                        ,"Candara"
                                                        ,"Century"
                                                        ,"Constantia"
                                                        ,"MS Mincho"};
        /// <summary>
        /// Ŭ���̾�Ʈ IP �Դϴ�.
        /// </summary>
        public static string s_ClientIP = "";
        /// <summary>
        /// ���̾ƿ� ���� �̸� �Դϴ�.
        /// </summary>
        public static string s_LayOutFileName = "\\RACTLayout.xml";
        /// <summary>
        /// ���� ���� �Դϴ�.
        /// </summary>
        //public static string s_Version = "1.1.2.10";
        // 2013-02-21 - shinyn - TACT �������� (����ȭ����.����߰�.���׼���.������Ƚ��)
        //public static string s_Version = "2.1.0.0";
        //public static string s_Version = "2.2.0.0"; //2017.06.21 - NoSeungPil - RCCS �α��� ����߰�
        //public static string s_Version = "2.2.1.0"; //2017.08.04 - KwonTaeSuk - �巡�׷� �������ý� �ڵ���ũ�� ��� �߰�
        //public static string s_Version = "2.2.2.0"; //20170818 - NoSeungPil - RCCS �α����� ��� ����� ������ ctrl + d ����
        //public static string s_Version = "2.2.3.0"; //20170822 - NoSeungPil - RCCS �α����� ��� �α������� ������ ����Ű ����
        //public static string s_Version = "2.3.0.0"; //20190212 ����ȭ ����(���Ȱ������,RPCS ���ݰ�������)
        //public static string s_Version = "2.3.0.1"; //20190326 ����ȭ ����(���Ȱ������ ���ɾ����� ��� ����,���� �� ��������,������� �α���,�α׾ƿ��ð� DB�������� ����)
        //public static string s_Version = "2.3.0.2"; //20190701 ����ȭ ����(���� �� ��������,RPCS ���ӱ��� �߰�)
        //public static string s_Version = "2.3.0.3"; //20190930 ����ȭ ����(����/�������� �� OneTerminal ����� ���� ǥ�� ��,OneTerminal �ɼ� �޴� ����,�ڵ����� ����ġ �ɼ��׸����� ���� ��� ����,��������ġ, G-PON-OLT, NG-PON-OLT ���ӽ� �߿���� ǥ��(FONT COLOR RED����)�� ����)
        //public static string s_Version = "2.3.0.4"; //20191007 ��������ġ, G-PON-OLT, NG-PON-OLT ���ӽ� �߿���� ǥ��(FONT COLOR RED����)�� ����)
        //public static string s_Version = "2.3.0.5"; //20191110  ���ɾ� ���� ������ ���� ���,Log ��� ���� ����
        //public static string s_Version = "2.3.0.6"; //20191118 LTE �ͳθ� ��û �ݺ� ����(�ѹ� ��û�ϰ� ��������� ���� ����),�ڳ� ������ ī��Ʈ 1->3
        //public static string s_Version = "2.3.0.7"; //20191209 ���ɾ� ���� ������ ���� ��� ����, ù ���ӽ� Ctrl+C,V �� ����ƮƮ �ν� ���� ����
        //public static string s_Version = "2.4.0.0"; // 2020-10-08 KwonTaeSUk [20����ȭ(.NET���׷��̵�)] .NET Framework 2.0 -> 4.8 (minor+1)
        //public static string s_Version = "2.4.0.1"; // 2021-04-15 KANGBONGHAN �ϰ�����â ���� ���ϸ��ɾ�,�������ɾ� ���� ����� �и�
        //public static string s_Version = "2.4.0.2"; // 2021-04-21 �͹̳� ���� ���� �ȵǴ� �κ� ���� // 2021-07-07KANGBONGHAN OneTerminal �ڵ���ũ�� ���� ����
        //public static string s_Version = "2.4.0.3"; // 2021-11-04 �͹̳� �÷��� ������ ����(80) �׸� ����
        //public static string s_Version = "2.4.0.4";   // 2022-01-12 RPCS �������� �� �߰� DSW105PR
        //public static string s_Version = "2.4.0.5";    //2022-08-18 Console��� �ø��󿬰�� �ٿ��ֱ� ���� Ȯ�� �� ����
        //public static string s_Version = "2.4.0.6";    //2022-09-01 ��ȸ���� �����̽� ����(Online��忡�� ���ӽ�(Telnet) TACT������ ���� ����
        //public static string s_Version = "2.4.0.7";    //2023-02-07 Online��忡�� ���ӽ�(ssh) TACT������ ���� ���� ����
        //public static string s_Version = "2.4.0.8";     //2023-02-24 ���ɾ� ù���� ���� �Ǵ� �ٿ��ֱ�� ����,ù���� ���� ���� ����
        //public static string s_Version = "2.4.0.9";   //2023-06 AGW 2001��Ʈ �����
        //public static string s_Version = "2.4.1.0";   //2024-03 ã��( Ctrl + F) ��� ��� �� ���ɾ� ���� �Ұ�, ������Ʈ() �̽�
        //public static string s_Version = "2.4.1.1";   //2025-01-20 More ó���� H�� ���� ���� ���� ��µǴ� ���� ����
        //public static string s_Version = "2.4.2.0";   //2025-03-12 CATV ���� ����
        //public static string s_Version = "2.4.2.1";   //2025-05-14 IP�˻� ���� �߰�(��ȿ�� �߰�, 3���� �̻� ��ȸ 3��° �ڸ� ������ �� ����� Like, 4��° �ڸ����� ���� ��� ��ġ��)
        //public static string s_Version = "2.4.2.2";   //2025-06-18 ���� �̽��� �������� ���� 
        //public static string s_Version = "2.4.2.3";   //2025-07-04 ��������� Console��� ���� 
        public static string s_Version = "2.4.2.4";   //2025-08-04 V4604S ���� ��Ʈ 23���� ����

        /// <summary>
        /// �α��� ��� �Դϴ�.
        /// </summary>
        public static LoginResultInfo s_LoginResult;
        /// <summary>
        /// ���� �� �Դϴ�.
        /// </summary>
        public static Form s_ClientMainForm;
        /// <summary>
        /// ����Ʈ����� ���� ���ݰ�ü�Դϴ�.
        /// </summary>
        public static MKRemote s_RemoteGateway = null;
        /// <summary>
        /// ���� IP �Դϴ�.
        /// </summary>
        //public static string s_ServerIP = "118.217.79.48";
        public static string s_ServerIP = "118.217.79.41";
        //public static string s_ServerIP = "118.217.79.15";
        //public static string s_ServerIP = "192.168.10.3";

        //public static string s_ServerIP = "127.0.0.1";
        /// <summary>
        /// ���� Port �Դϴ�.
        /// </summary>
        public static int s_ServerPort = 43210;
        /// <summary>
        /// ���α׷� �̸� �Դϴ�.
        /// </summary>
        public static string s_ProgramName = "TACT Client";
        /// <summary>
        /// Mac Address �Դϴ�.
        /// </summary>
        public static string s_MacAddress = "";
        /// <summary>
        /// ���� ���� ���� �Դϴ�.
        /// </summary>
        public static bool s_IsServerConnected = false;
        /// <summary>
        /// ���� ������ �˻� �ֱ� �Դϴ�(���� : �и���)
        /// </summary>
        public static int s_ServerCheckInterval = 100;
        /// <summary>
        /// ��û ť �Դϴ�.
        /// </summary>
        public static Queue<CommunicationData> s_RequestQueue = new Queue<CommunicationData>();

        /// <summary>
        /// 2013-05-03-shinyn - ���õ� ���ٱ��� ����Դϴ�.
        /// </summary>
        public static TreeNodeEx m_SelectedSystemNode = null;

        /// <summary>
        /// 2013-09-09- shinyn - ���õ� ����� ����Դϴ�.
        /// </summary>
        public static TreeNodeEx m_SelectedUserNode = null;



        /// <summary>
        /// ����� ����
        /// </summary>
        public static string s_UserAccount;
        /// <summary>
        /// ����� ��й�ȣ
        /// </summary>
        public static string s_Password;
        /// <summary>
        /// ä�� �̸� �Դϴ�.
        /// </summary>
        public static string s_ChannelName = "RemoteClient";
        /// <summary>
        /// ��û�� ��� �Դϴ�.
        /// </summary>
        public static Dictionary<int, ISenderObject> s_SenderList = new Dictionary<int, ISenderObject>();
        /// <summary>
        /// ����� �׷� ��� �Դϴ�.
        /// </summary>
        public static GroupInfoCollection s_GroupInfoList = new GroupInfoCollection();

        /// <summary>
        /// 2013-08-14- shinyn -����ڸ���Ʈ�� ����� �׷� ����Դϴ�.
        /// </summary>
        public static UserInfoCollection s_UserInfoList = new UserInfoCollection();


        /// <summary>
        /// ���� ���� ��� �Դϴ�.
        /// </summary>
        public static ShortenCommandGroupInfoCollection s_ShortenCommandList = new ShortenCommandGroupInfoCollection();
        /// <summary>
        /// ��ũ��Ʈ ��� �Դϴ�.
        /// </summary>
        public static ScriptGroupInfoCollection s_ScriptList = new ScriptGroupInfoCollection();
        /// <summary>
        /// ������ ���� ó�� �Դϴ�.
        /// </summary>
        public static DataSyncProcessor s_DataSyncProcssor = new DataSyncProcessor();
        /// <summary>
        /// �� ��� �Դϴ�.
        /// </summary>
        public static ModelInfoCollection s_ModelInfoList = new ModelInfoCollection();
        /// <summary>
        /// ���� ���ɾ� ��� �Դϴ�.
        /// </summary>
        public static LimitCmdInfoCollection s_LimitCmdInfoList = new LimitCmdInfoCollection();
        /// <summary>
        /// �⺻ ���ɾ� ��� �Դϴ�.
        /// </summary>
        public static DefaultCmdInfoCollection s_DefaultCmdInfoList = new DefaultCmdInfoCollection();
        /// <summary>
        /// �ڵ��ϼ�  ��� �Դϴ�.
        /// </summary>
        public static AutoCompleteCmdInfoCollection s_AutoCompleteCmdList = new AutoCompleteCmdInfoCollection();
        /// <summary>
        /// ��� ���� ����Դϴ�.
        /// </summary>
        public static ArrayList s_DevicePartList = new ArrayList();
        /// <summary>
        /// ������� ��� �Դϴ�.
        /// </summary>
        //public static DeviceInfoCollection s_DeviceInfoList = new DeviceInfoCollection();
        /// <summary>
        /// Ŭ���̾�Ʈ �ɼ� ���� �Դϴ�.
        /// </summary>
        public static ClientOption s_ClientOption = null;
        /// <summary>
        /// ���� ���� �Դϴ�.
        /// </summary>
        public static OrganizationInfo s_OrganizationInfo;
        /// <summary>
        /// ã�� �� �Դϴ�.
        /// </summary>
        // public static TelnetFindForm  s_TelnetFindForm = null;
        /// <summary>
        /// �͹̳� �α� ó�� ���μ��� �Դϴ�.
        /// </summary>
        public static CommandExecuteLogProcess s_TerminalExecuteLogProcess;
        /// <summary>
        /// �ڳ� ���� ��� �Դϴ�.
        /// </summary>
        public static Dictionary<int, DaemonProcessRemoteObject> s_DaemonProcessList = new Dictionary<int, DaemonProcessRemoteObject>();
        /// <summary>
        /// Serial ó�� ���μ��� �Դϴ�.
        /// </summary>
        public static SerialProcess s_SerialProcessor;
        /// <summary>
        /// �ڳ� ó�� ���μ��� �Դϴ�.
        /// </summary>
        public static TelnetProcessor.TelnetProcessor s_TelnetProcessor;
        /// <summary>
        /// ��� �������� ���� �� ������� ���� �Դϴ�.
        /// </summary>
        public static bool s_IsModeChangeConnect = false;
        /// <summary>
        /// ��� ���� �� �Դϴ�.
        /// </summary>
        public static ModeChangeSubForm s_ModeChangeForm = new ModeChangeSubForm();
        /// <summary>
        /// ���� �α� ���μ��� �Դϴ�.
        /// </summary>
        public static RACTClient.Logging.FastLogger s_FileLogProcessor = null;
        /// <summary>
        /// ��û ��� Ÿ�� �ƿ� �Դϴ�.
        /// </summary>
        public static readonly int s_RequestTimeOut = 5000;
        /// <summary>
        /// ���α׷� ���� ���� �Դϴ�.
        /// </summary>
        public static bool s_IsProgramShutdown = false;

        /// <summary>
        /// �ڵ����� ���� �Դϴ�.
        /// </summary>
        public static bool s_IsAutoSaveLog = false;

        //2017.06.21 - NoSeungPil - RCCS �α��� ����߰�
        /// <summary>
        /// ���ӹ�� �Դϴ�. (1:RCCS �α���)
        /// </summary>
        public static int s_ConnectionMode = 0;

        /// <summary>
        /// RCCS �α��� üũ
        /// </summary>
        public static bool s_IsRCCSLoginOK = false;

        /// <summary>
        /// RCCS ����� IP �Դϴ�.
        /// </summary>
        public static string s_RCCSIP;

        /// <summary>
        /// RCCS ����� ���� Port��ȣ �Դϴ�.
        /// </summary>
        public static int s_RCCSPort = 0;

        /// <summary>
        /// RPCS ����� IP �Դϴ�.
        /// </summary>
        public static string s_RPCSIP;

        /// <summary>
        /// RPCS ����� ���� Port��ȣ �Դϴ�.
        /// </summary>
        public static int s_RPCSPort = 0;

        public static int s_MultipleCmd = 20;

        public static E_IpType m_ViewIPType = E_IpType.ALL;

        /// <summary>
        /// �ｺ üũ�� ó���մϴ�.
        /// </summary>
        /// <returns></returns>
        public static bool AreYouThere()
        {
            return true;
        }

        /// <summary>
        /// �α��� �� ���� ������ ó�� �մϴ�.
        /// </summary>
        /// <param name="vID">����� ���̵� �Դϴ�.</param>
        /// <param name="vPwd">����� �н����� �Դϴ�.</param>
        /// <param name="vIPAddress">����� ������ �ּ� �Դϴ�.</param>
        public static bool LoginConnect()
        {
            string tLogMessage = "";

            try
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat(AppGlobal.s_UserAccount, " �������� �α��� �մϴ�."));
                RemoteClientMethod tSPO = (RemoteClientMethod)AppGlobal.s_RemoteGateway.ServerObject;

                // 2014-03-19 - ������ - ������ �����ؼ� �α��ν� DB���� sha256���� ��ȣȭ�� ��й�ȣ�� �α��εǵ��� �ϰ�,
                // ���̵�,��й�ȣ �Է��ؼ� �α��ν� ��й�ȣ�� sha256���� ��ȣȭ�Ͽ� �α��εǵ��� ����
                string password = Hash.GetHashPW(AppGlobal.s_Password);
                if (EncryptGlobal.s_shaYN == "1")
                {
                    password = AppGlobal.s_Password;
                    EncryptGlobal.s_shaYN = "0";
                }
                //20131118 �赵�� �н����带 SHA-256 UTF-8 ������� �ѱ�
                s_LoginResult = (LoginResultInfo)ObjectConverter.GetObject(tSPO.CallUserLoginMethod(AppGlobal.s_UserAccount, password, AppGlobal.s_ClientIP, AppGlobal.s_Caller));
                if (s_ClientMainForm is ClientMain)
                {
                    ((ClientMain)s_ClientMainForm).SetMainFormText("TACT Ŭ���̾�Ʈ (Ver " + AppGlobal.s_Version + ") :::");// + AppGlobal.m_ServerIP + " (���� Ver " + m_LoginResult.ServerVersion + ")");
                }


                if (s_LoginResult.LoginResult != E_LoginResult.Success)
                {
                    switch (AppGlobal.s_LoginResult.LoginResult)
                    {
                        case E_LoginResult.IncorrectID:
                            AppGlobal.ShowMessage("������ Ȯ���� �ּ���.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "������ Ȯ���� �ּ���. ");
                            break;
                        case E_LoginResult.IncorrectPassword:
                            AppGlobal.ShowMessage("��й�ȣ�� Ȯ���� �ּ���.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "��й�ȣ�� Ȯ���� �ּ���. ");
                            //Application.Exit();
                            break;
                        case E_LoginResult.AlreadyLogin:
                            if (s_Caller == E_TerminalMode.RACTClient)
                            {
                                AppGlobal.ShowMessage("���� ������ �̹� �α��� �Ǿ� �ֽ��ϴ�. Ŭ���̾�Ʈ�� ���� ���� �Ͽ������ 30�� ���Ŀ� �ٽ� ���� �Ͻʽÿ�.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "�̹� �α��� �Ǿ� �ֽ��ϴ�. ");

                                // AppGlobal.s_ClientMainForm.Close();
                                break;
                            }
                            else
                            {
                                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "�α��ο� " + E_LoginResult.Success.ToString() + "�߽��ϴ�.");
                                AppGlobal.s_IsServerConnected = true;
                                return true;
                            }
                        case E_LoginResult.UnknownError:
                            AppGlobal.ShowMessage("������ �����ϴ�.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "������ �����ϴ�.");
                            //Application.Exit();
                            break;
                        case E_LoginResult.NotAuthentication:
                            AppGlobal.ShowMessage("TACT ��� ������ �����ϴ�.\n�����ڿ��� ���� �ϼ���.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TACT ��� ������ �����ϴ�.");
                            // Application.Exit();
                            break;
                        case E_LoginResult.UnUsedLimit:
                            AppGlobal.ShowMessage("TACT ���� ���� ��¥�� �������ϴ�.\n�����ڿ��� ���� �ϼ���.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TACT ���� ���� ��¥�� �������ϴ�..");
                            break;
                        default:
                            AppGlobal.ShowMessage("�ߴ��� ������ �߻� �޽��ϴ�.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "�ߴ��� ������ �߻� �޽��ϴ�.");
                            // Application.Exit();
                            break;
                    }
                    Application.ExitThread();
                    // Application.Exit();
                    return false;
                }
                else
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "�α��ο� " + s_LoginResult.LoginResult.ToString() + "�߽��ϴ�.");
                    AppGlobal.s_IsServerConnected = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "������ ������ �� �����ϴ�.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
            return false;
        }




        /// <summary>
        /// ���� ������ ������ �õ� �մϴ�.
        /// </summary>
        /// <returns>���� �õ� ���� ���� �Դϴ�.</returns>
        public static E_ConnectError TryDaemonConnect(string aDaemonIP, int aDaemonPort, string aDaemonChannelName, out MKRemote oRemoteObject)
        {
            int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;
            MKRemote tRemote;
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat("Daemon(", aDaemonIP, ":", aDaemonPort, ":", aDaemonChannelName, ") �� ���� �մϴ�."));
            oRemoteObject = new MKRemote(E_RemoteType.TCPRemote, aDaemonIP, aDaemonPort, aDaemonChannelName);


            if (oRemoteObject == null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("Daemon(", aDaemonIP, ":", aDaemonPort, ":", aDaemonChannelName, ") �� ���� �� �� �����ϴ�."));
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
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("Daemon�� ������ �� �����ϴ�. Daemon�� ���������� ���۵Ǿ����� �Ǵ� FireWall�� �۵������� Ȯ�� �Ͻʽÿ�. :", tErrorString));
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
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("Daemon�� ������ �� �����ϴ�. Daemon�� ���������� ���۵Ǿ����� �Ǵ� FireWall�� �۵������� Ȯ�� �Ͻʽÿ�. :", tErrorString));
                            return E_ConnectError.LinkFail;
                        }
                    }
                }

                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat("Daemon(", aDaemonIP, ":", aDaemonPort, ":", aDaemonChannelName, ") �� ���� �߽��ϴ�."));
                return E_ConnectError.NoError;
            }
        }

        /// <summary>
        /// ������ ������ �õ� �մϴ�.
        /// </summary>
        /// <returns>���� �õ� ���� ���� �Դϴ�.</returns>
        public static E_ConnectError TryServerConnect()
        {
            int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;

            if (s_RemoteGateway == null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat("����(", s_ServerIP, ":", s_ServerPort, ")�� ���� �մϴ�."));
                s_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, s_ServerIP, s_ServerPort, s_ChannelName);
                //test code
                // s_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, "192.168.25.4", s_ServerPort, s_ChannelName);
            }

            if (s_RemoteGateway == null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, string.Concat("����(", s_ServerIP, ":", s_ServerPort, ")�� ���� �� �� �����ϴ�."));
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
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "������ ������ �� �����ϴ�. ������ ���������� ���۵Ǿ����� �Ǵ� FireWall�� �۵������� Ȯ�� �Ͻʽÿ�.");
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
        /// ���� �޽��� �ڽ��Դϴ�.
        /// </summary>
        public static DialogResult ShowMessage(string vMessage, MessageBoxButtons vButtonType, MessageBoxIcon vIconType)
        {
            DialogResult result = MessageBox.Show(vMessage, AppGlobal.s_ProgramName, vButtonType, vIconType);
            return result;
        }

        /// <summary>
        /// ���� �޽��� �ڽ��Դϴ�.
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
        /// ���� �޽��� �ڽ��Դϴ�.
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
        /// ���� �޽��� �ڽ��Դϴ�.
        /// </summary>
        public static DialogResult ShowMessage(string vMessage)
        {
            DialogResult result = MessageBox.Show(vMessage, AppGlobal.s_ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }

        /// <summary>
        /// ���� �޽��� �ڽ��Դϴ�.
        /// </summary>
        public static DialogResult ShowMessage(System.Windows.Forms.IWin32Window vForm, string vMessage)
        {
            DialogResult result = ShowMessage(vForm, vMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }

        /// <summary>
        /// �⺻ ��û ������ �����մϴ�.
        /// </summary>
        /// <returns>������ ��û ���� ��ü �Դϴ�.</returns>
        public static RequestCommunicationData MakeDefaultRequestData()
        {
            RequestCommunicationData tRequestData = new RequestCommunicationData();
            tRequestData.ClientID = AppGlobal.s_LoginResult.ClientID;
            return tRequestData;
        }

        /// <summary>
        /// ��û �����͸� �����մϴ�.
        /// </summary>
        /// <param name="vSender">������ �Դϴ�.</param>
        /// <param name="vCommunicationData">���� ������ �Դϴ�.</param>
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
            //2013-05-02 - shinyn - ��û���۽� ManualSet�Ѵ�.
            //m_MRE.Set();
        }



        /// <summary>
        /// ��û �����ڸ� �߰� �մϴ�.
        /// </summary>
        /// <param name="vSender">������ �Դϴ�.</param>
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

        // shinyn - 2012-12-13 - NE Group ID int -> string ���� 'B' PON(Biz) -> FOMs���� ���� ���� ����
        /// <summary>
        /// �Ű������� ��ġ�ϴ� �̸��� �׷������� ������ ��ȯ�մϴ�.
        /// </summary>
        /// <param name="aGroupName">ã���� �ϴ� �׷������� �̸��Դϴ�.</param>
        /// <returns>��ġ�ϴ� ������ ������ �׷�������, ��ġ�ϴ� ������ ������ null�� ��ȯ�մϴ�.</returns>
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
        /// 2013-08-13- shinyn - ������ �׷��� ������ �����ϰ� ����Ʈ�� �Ѵ�.
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

            // 2013-09-09 - shinyn- ����� �׷�� ��񸮽�Ʈ�� �ε�
            if (aTreeType == E_TreeType.UserGroup || aTreeType == E_TreeType.UserGroupList)
            {
                if (AppGlobal.s_GroupInfoList == null) return;

                vTreeView.Nodes.Clear();
                int tDeviceCount = 0;
                try
                {
                    vTreeView.Visible = false;

                    // 2013-08-13 - �ܰ躰 �׷츮��Ʈ�� �ƴ� �����ҽ�
                    /*
                    foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
                    {
                        tDeviceCount = AppGlobal.s_GroupInfoList.GetCountByGroup(tGroupInfo.ID);

                        tGroupNode = new TreeNodeEx(string.Concat(tGroupInfo.Name, "[", tDeviceCount, "]"), 0, 0);
                        tGroupNode.Tag = tGroupInfo;
                        //��Ʈ��ũ �� ��� �߰�
                        vTreeView.Nodes.Add(tGroupNode);

                        foreach (DeviceInfo tDeviceInfo in tGroupInfo.DeviceList.InnerList)
                        {
                            tDeviceNode = new TreeNodeEx(string.Concat(tDeviceInfo.Name.Trim(), "[", tDeviceInfo.IPAddress, "]"), 1, 1);
                            tDeviceNode.Tag = tDeviceInfo;
                            tGroupNode.Nodes.Add(tDeviceNode);
                        }
                    }
                    */
                    // 2013-08-13 - �ܰ躰 �׷츮��Ʈ ǥ��



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
                            //��Ʈ��ũ �� ��� �߰�
                            System.Diagnostics.Debug.WriteLine(tGroupInfo.Name + " �׷� �߰�");
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

                //���� ������ ����� �մϴ�.
                vTreeView.Nodes.Clear();
                Hashtable tGroupHash = null;
                int tDeviceCount = 0;

                try
                {
                    vTreeView.Visible = false;
                    tGroupNode = new TreeNodeEx("Root", 0, 0);
                    tGroupNode.Tag = AppGlobal.s_OrganizationInfo.AllGroupInfo;
                    //��Ʈ��ũ �� ��� �߰�
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

                    // 2013-08-13 - �ܰ躰 �׷츮��Ʈ ǥ��
                    foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
                    {
                        //tDeviceCount = AppGlobal.s_GroupInfoList.GetCountByGroup(tGroupInfo.ID);
                        tDeviceCount = tGroupInfo.DEVICE_COUNT;

                        tGroupNode = new TreeNodeEx(string.Concat(tGroupInfo.Name, "[", tDeviceCount, "]"), 0, 0);
                        tGroupNode.Tag = tGroupInfo;

                        // �������� ���� �׷��� �ִ°�� ������ �ʵ��� ó���Ѵ�.
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
                            //��Ʈ��ũ �� ��� �߰�
                            System.Diagnostics.Debug.WriteLine(tGroupInfo.Name + " �׷� �߰�");
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

                    // 2013-08-13 - ����ڿ� ���� ������ �׷츮��Ʈ ǥ��
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
        /// ��Ʈ���� �ʱ�ȭ �մϴ�.
        /// </summary>
        /// <param name="vTV">��Ʈ���� �ʱ�ȭ �� Ʈ���� �Դϴ�.</param>
        public static void InitializeGroupTreeView(TreeViewEx vTreeView, E_TreeType aTreeType)
        {
            InitializeGroupTreeView(vTreeView, aTreeType, null);
        }



        /// <summary>
        /// ������ ����� ������ ��� �׷��� �߰��մϴ�.
        /// </summary>
        /// <param name="vGroupHash">�׷� �ؽ� ���̺� �Դϴ�.</param>
        /// <param name="vParentNode">���� ��� �Դϴ�.</param>
        /// <param name="vGroupInfo">�׷� ���� �Դϴ�.</param>
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
        /// �׸��� ��Ÿ���� �ʱ�ȭ �մϴ�.
        /// </summary>
        /// <param name="vGrid">��Ÿ���� �ʱ�ȭ �� �׸��� �Դϴ�.</param>
        public static void InitializeGridStyle(C1FlexGrid vGrid)
        {
            vGrid.Styles["Normal"].BackColor = Color.FromArgb(255, 255, 255);
            vGrid.Styles["Normal"].ForeColor = Color.FromArgb(75, 75, 75); //�׸��� �ȿ� �ִ� ��Ʈ

            vGrid.Styles["Normal"].Border.Color = Color.FromArgb(225, 232, 242); //cell color		
            vGrid.Styles["Fixed"].BackColor = Color.FromArgb(245, 245, 245);
            vGrid.Styles["Fixed"].ForeColor = Color.FromArgb(0, 0, 0);
            vGrid.Styles["Fixed"].Border.Style = BorderStyleEnum.Flat;
            vGrid.Styles["Fixed"].Border.Color = Color.FromArgb(201, 201, 201);//�ӽ÷� ó����.
            vGrid.Styles["EmptyArea"].BackColor = Color.FromArgb(246, 246, 246); //�����Ͱ� ���� ��� backcolor

            vGrid.Styles["Highlight"].ForeColor = Color.FromArgb(255, 255, 255);
            vGrid.Styles["Highlight"].BackColor = Color.FromArgb(110, 100, 189);
        }

        /// <summary>
        /// �޺��ڽ� ��Ÿ���� �ʱ�ȭ �մϴ�
        /// </summary>
        /// <param name="cboDevicePart">�޺��ڽ� �Դϴ�.</param>
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
            aComboBox.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            aComboBox.ForeColor = System.Drawing.Color.Black;
            aComboBox.ImageList = null;
            aComboBox.ItemHeight = 14;
            aComboBox.MaxDorpDownWidth = 500;
            aComboBox.SelectedIndex = -1;
            aComboBox.ShowColorBox = false;
        }


        /// <summary>
        /// ��� �з��� ��� �� ��� �Դϴ�.
        /// </summary>
        public static Hashtable m_DeviceModelInfoDevicePart = new Hashtable();
        /// <summary>
        /// ��� ���� ����� �ʱ�ȭ �մϴ�.
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
                //��� �з��� �� ����� ���� �մϴ�.
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
        /// ��ü�� ������ �޸𸮽�Ʈ���� ��ȯ �մϴ�.
        /// </summary>
        /// <param name="aValue">������ ��ü �Դϴ�.</param>
        /// <returns>�޸� ��Ʈ�� �Դϴ�.</returns>
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
        /// ��� �з� �޺��ڽ� �ʱ�ȭ �޼���
        /// </summary>
        /// <param name="aISManagementList">�����Ǵ� ���з��� ǥ�������ǿ��� </param>
        /// <param name="vcmbDivision">�з� �޺��ڽ�</param>
        public static void InitializeDevicePartComboBox(bool aISManagementList, MKComboBox vDevicePartComboBox)
        {
            InitializeDevicePartComboBox(aISManagementList, vDevicePartComboBox, true, null);
        }

        /// <summary>
        /// ��� �з� �޺��ڽ� �ʱ�ȭ �޼���
        /// </summary>
        /// <param name="aISManagementList">�����Ǵ� ���з��� ǥ�������ǿ��� </param>
        /// <param name="vcmbDivision">�з� �޺��ڽ�</param>
        /// <param name="aAllType">��ü ���з������� ����</param>
        /// <param name="aSelectType">���õ� ���з� ����Ʈ(,�� �и�)</param>
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

            // ���ĺ� ������ �׸��� ����.
            Array.Sort(tConditions, tItems);

            vDevicePartComboBox.Items.Clear();
            vDevicePartComboBox.Items.Add("- �з� ���� -");
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
        /// ��� �� �޺� �ڽ��� �ʱ�ȭ �մϴ�.
        /// </summary>
        /// <param name="vDeviceModelComboBox">��� �� �޺� �ڽ� �Դϴ�.</param>
        /// <param name="vDevicePart">��� �з� �Դϴ�.</param>
        public static void InitializeDeviceModelComboBox(MKComboBox vDeviceModelComboBox, string vDevicePart)
        {
            MKListItem tListItem = null;
            vDeviceModelComboBox.Enabled = false;
            vDeviceModelComboBox.Items.Clear();
            vDeviceModelComboBox.Items.Add("- �� ���� -");

            if (m_DeviceModelInfoDevicePart[vDevicePart] == null) return;

            ModelInfo[] tItems = new ModelInfo[((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart]).Count];

            string[] tConditions = new string[((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart]).Count];
            for (int i = 0; i < ((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart]).Count; i++)
            {
                tConditions[i] = ((ModelInfo)((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart])[i]).ModelName;
                tItems[i] = (ModelInfo)((ArrayList)m_DeviceModelInfoDevicePart[vDevicePart])[i];
            }

            // ���ĺ� ������ �׸��� ����.
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
            //2008-05-01 hanjiyeon ���� - ��� ��Ӵٿ��Ʈ�� �������� ���Ľ��� ǥ�� - end
            vDeviceModelComboBox.Enabled = true;
        }

        /// <summary>
        /// ��Ʈ��ũ �� �ڵ带 �̿��Ͽ� ��Ʈ��ũ �� �̸��� �������� �մϴ�.
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
        /// �� �ڵ带 �̿��Ͽ� �� ���� �������� �մϴ�.
        /// </summary>
        /// <param name="aIsBranchCode">�������ڰ� �����ڵ������� �����Դϴ�.false:�����ڵ�</param>
        /// <param name="vBranchCode">�����ڱ� �Դϴ�.</param>
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
        /// ���� �ڵ���� �̿��Ͽ� ���͸��� �������� �մϴ�.
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
        /// ���� �ڵ带 �̿��Ͽ� ��������(��)�� �����ɴϴ�. ��������� ����.
        /// </summary>
        /// <returns></returns>
        public static FACTGroupInfo GetGroupInfoByCenterCode(string aCenterCode)
        {
            FACTGroupInfo tGroupInfo = null;

            if (AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups.Count > 0)
            {
                for (int i = 0; i < AppGlobal.s_OrganizationInfo.AllGroupInfo.SubGroups.Count; i++)
                {
                    //��Ʈ��ũ ��
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
        /// ��ư ��Ÿ���� �����մϴ�.
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
        /// ��ư�� �̹��� ��Ÿ���� �����մϴ�.
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
        /// XML ������ ���� �մϴ�.
        /// </summary>
        public static void MakeClientOption()
        {
            // 2013-04-24- shinyn - ȯ�漳������ ����� ����üũ
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
        /// ��ũ��Ʈ ������ �����մϴ�.
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
        /// ���α׷����� ǥ�ø� ó�� �մϴ�.
        /// </summary>
        /// <param name="aVisable"></param>
        public static void ShowLoadingProgress(bool aVisable)
        {
            ((ClientMain)AppGlobal.s_ClientMainForm).ShowLoadingProgress(aVisable);
        }

        /// <summary>
        /// 2013-01-11 - shinyn - R14-01 - ��� ��� ���� 
        /// </summary>
        /// <param name="aDeviceInfos"></param>
        /// <returns></returns>
        public static bool SaveDeviceList(UserInfo aUserInfo, DeviceInfoCollection aDeviceInfos, DeviceCfgSaveInfoCollection aDviceCfgSaveInfos)
        {
            /*   
             -- XML����
            <?xml version="1.0" ?> 
            <Account></Account>
            <DeviceInfos>
              <DeviceIDs>
                  <DeviceID value="211987">
    	                <Name>����̸�</Name>
                        <ModelID>�𵨾��̵�</ModelID>
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
    	                <Name>����̸�</Name>
                        <ModelID>�𵨾��̵�</ModelID>
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
                               "�����.XML";


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
        /// 2013-01-11 - shinyn - CFG ���������� Script ���·� ��ȯ�Ѵ�.
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
        /// 2013-05-02 - shinyn - ��ũ��񿬰ῡ �ʿ��� Telnet���� ��ũ��Ʈ�� �����.
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
             -- XML����
            <?xml version="1.0" ?> 
            <Account></Account>
            <DeviceInfos>
              <DeviceIDs>
                  <DeviceID value="211987">
    	                <Name>����̸�</Name>
                        <ModelID>�𵨾��̵�</ModelID>
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
    	                <Name>����̸�</Name>
                        <ModelID>�𵨾��̵�</ModelID>
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
        /// 2013-04-19 - shinyn - Grid���� �ٿ��ֱ� �Ұ�� �����
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
        /// 2013-04-19 - shinyn - Grid���� �ٿ��ֱ� �Ұ�� �����
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
                                //mjjoe, 2008.10.21 �ڵ��߰� ---------------------------------------------------------------------------------
                                int tAddNewOption_AddLine;
                                if (vGrid.AllowAddNew == true)
                                    tAddNewOption_AddLine = 2;
                                else
                                    tAddNewOption_AddLine = 1;

                                Row tAddedRow;
                                //mjjoe, 2008.10.21 �ڵ��߰� ��---------------------------------------------------------------------------------
                                for (int i = 0; i < tStringLine.Length; i++)
                                {
                                    //mjjoe, 2008.10.21 �ڵ��߰� ---------------------------------------------------------------------------------
                                    //Ŭ������ ����� �������� ������ �������� ����Ǿ� ����.
                                    //(Excel���� �׽�Ʈ). �׷��Ƿ�, ������ ���� �� ���̸� Line�� �߰����� �ʵ��� ó���Ѵ�.
                                    if (i == tStringLine.Length - 1 && tStringLine[i] == "")
                                        break;
                                    //mjjoe, 2008.10.21 �ڵ��߰� �� ---------------------------------------------------------------------------------

                                    //mjjoe, 2008.10.21 ���� ---------------------------------------------------------------------------------
                                    //if(tRow >= vGrid.Rows.Count) //old�ڵ�
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

                                        //vGrid[tRow, tCol + vGrid.Col] = tValues[tCol].Replace(((char)13).ToString(), ""); //old�ڵ�
                                        tAddedRow[tCol + vGrid.Col] = tValues[tCol].Replace(((char)13).ToString(), "");
                                    }
                                    //mjjoe, 2008.10.21 ���� ��---------------------------------------------------------------------------------

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

        //2015-10-28 hanjiyeon �߰�
        /// <summary>
        /// ��ī�ڷ罼Ʈ ��������� ���θ� ��ȯ�մϴ�.
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

        //2018-11-20 KangBonghan �߰�
        /// <summary>
        /// RPCS ��������� ���θ� ��ȯ�մϴ�.
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
            int validPartCount = 0; // ��ȿ�� ���� ����

            for (int i = 0; i < rawParts.Length; i++)
            {
                string part = rawParts[i];

                // �߰��� �� ���� ������ ���� (��: 210..10.5)
                if (string.IsNullOrWhiteSpace(part))
                {
                    if (i != rawParts.Length - 1)
                        return false;

                    part = "0"; // ������ �� ������ 0���� ����
                }
                else
                {
                    validPartCount++; // ��ȿ�� ���̸� ī��Ʈ
                }

                segments.Add(part);
            }

            // �� ������ ����: ��ȿ�� ���� ������ 3�� �̻��̾�� ��
            if (validPartCount < 3)
                return false;

            // �ִ� 4���ݱ��� 0���� ����
            while (segments.Count < 4)
                segments.Add("0");

            if (segments.Count > 4)
                return false;

            // ���� �� ���� �˻�
            for (int i = 0; i < 4; i++)
            {
                string seg = segments[i];

                if (!seg.All(Char.IsDigit))
                    return false;

                int val;
                if (!int.TryParse(seg, out val) || val < 0 || val > 255)
                    return false;

                segments[i] = val.ToString(); // ���ڸ� 0 ����
            }

            returnip = string.Join(".", segments);
            return true;
        }

    }
}
