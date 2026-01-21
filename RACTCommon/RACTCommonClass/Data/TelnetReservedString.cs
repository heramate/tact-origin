using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RACTCommonClass
{
    #region 예약 문자열 정의 부분 입니다 ---------------------------------------------------
    /// <summary>
    /// 예약어 정보 관리 클래스 입니다.
    /// </summary>
    public class ReservedString
    {
        public string Command = "";
        public string Description = "";

        public ReservedString(string aCommand, string aDescription)
        {
            Command = aCommand;
            Description = aDescription;
        }
    }

    /// <summary>
    /// Telnet명령 처리를 위한 예약 문자열입니다.
    /// </summary>
    public class TelnetReservedString
    {
        /// <summary>
        /// Telnet사용자 ID입니다.
        /// </summary>
        public const string c_UserID1 = "${USERID1}";
        /// <summary>
        /// Telnet사용자 암호 입니다.
        /// </summary>
        public const string c_Password1 = "${PASSWORD1}";
        /// <summary>
        /// Telnet사용자 ID2입니다.
        /// </summary>
        public const string c_UserID2 = "${USERID2}";
        /// <summary>
        /// Telnet사용자 암호2입니다.
        /// </summary>
        public const string c_Password2 = "${PASSWORD2}";
        /// <summary>
        /// 공백 입니다.
        /// </summary>
        public const string c_Space = "${SPACE}";
        /// <summary>
        /// 엔터 입니다.
        /// </summary>
        public const string c_Enter = "${ENTER}";
        /// <summary>
        /// FTP서버 주소 입니다.
        /// </summary>
        public const string c_FTPIP = "${FTPIP}";
        /// <summary>
        /// FTP사용자 ID입니다.
        /// </summary>
        public const string c_FTPUSER = "${FTPUSER}";
        /// <summary>
        /// FTP사용자 암호 입니다.
        /// </summary>
        public const string c_FTPPASSEORD = "${FTPPASSWORD}";
        /// <summary>
        /// TFTP서버 주소 입니다.
        /// </summary>
        public const string c_TFTPIP = "${TFTPIP}";
        /// <summary>
        /// OS파일 이름 입니다.
        /// </summary>
        public const string c_OSFILENAME = "${OSFILENAME}";
        /// <summary>
        /// OS파일 크기 입니다.
        /// </summary>
        public const string c_OSFILESIZE = "${OSFILESIZE}";
        /// <summary>
        /// OS 버전 입니다.
        /// </summary>
        public const string c_VERSION = "${VERSION}";
        /// <summary>
        /// 장비의 현재 OS버전 입니다.
        /// </summary>
        public const string c_DEVICEOSVERSION = "${DEVICEOSVERSION}";
        /// <summary>
        /// 작업 종료 입니다.
        /// </summary>
        public const string c_EXIT = "${EXIT}";
        /// <summary>
        /// 문자열이 포함된 명령은 Enter를 입력하지않습니다.
        /// </summary>
        public const string c_NoEnter = "~";
        /// <summary>
        /// PingTest명령 입니다.
        /// </summary>
        public const string c_PINGTEST = "${PINGTEST}";
        /// <summary>
        /// 변수의 시작 문자열 입니다.
        /// </summary>
        public const string c_VARIABLESTRING = "$V{";
        /// <summary>
        /// 구분자 입니다.
        /// </summary>
        public const string c_Seperator = "|";
        /// <summary>
        /// 기다립니다.
        /// </summary>
        public const string c_WAIT = "${WAIT}";

        //mjjoe 2009기능고도화 - 추가명령어 timeout
        /// <summary>
        /// 지정한 초만큼 명령어 Timeout 시간을 설정합니다.
        /// </summary>
        public const string c_TIMEOUT = "${TIMEOUT}";
        /// <summary>
        /// 점검 스케줄링(CONFIG백업)결과를 FTP서버에 백업하는 명령어 입니다.
        /// </summary>
        public const string c_FTPUPLOAD = "${FTPUPLOAD}";
        /// <summary>
        /// 점검 스케줄링(CONFIG백업)결과를  DB서버에 백업하는 명령어 입니다.
        /// </summary>
        public const string c_DBUPLOAD = "${DBUPLOAD}";
        /// <summary>
        /// 확장자 및 경로를 제외한 환경파일이름 입니다.
        /// </summary>
        public const string c_CONFIGFILENAME_IP = "${IPFILENAME}";
        /// <summary>
        /// 확장자가 포함된 환경파일이름 입니다.
        /// </summary>
        public const string c_CONFIGFILENAMEEXT_IP = "${IPFILENAMEEXT}";
        //jwmin 2009기능고도화 - 점검 스케줄링(CONFIG비교) 명령에 사용되는 예약어로 CONFIG비교 작업을 실행하는 명령어 입니다.
        public const string c_CFGCOMPARE = "${COMPARE}";
        /// <summary>
        /// 확장자 및 경로를 제외한 환경파일이름 입니다.
        /// </summary>
        public const string c_CONFIGFILENAME = "${CONFIGFILENAME}";
        /// <summary>
        /// 확장자가 포함된 환경파일이름 입니다.
        /// </summary>
        public const string c_CONFIGFILENAMEEXT = "${CONFIGFILENAMEEXT}";
        /// <summary>
        /// 확장자 및 경로를 제외한 환경파일이름 입니다.(16자리 제한)
        /// </summary>
        public const string c_CONFIGFILENAME16 = "${CONFIGFILENAME16}";
        /// <summary>
        /// 확장자가 포함된 환경파일이름 입니다.(16자리 제한)
        /// </summary>
        public const string c_CONFIGFILENAMEEXT16 = "${CONFIGFILENAMEEXT16}";
        /// <summary>
        /// 경로및 확장자까지 포함된 환경파일 이름 입니다.
        /// </summary>
        public const string c_CONFIGFULLFILENAME = "${CONFIGFULLFILENAME}";
        /// <summary>
        /// 서버의 IP주소 입니다.
        /// </summary>
        public const string c_ServerIP = "${IP}";
        /// <summary>
        /// 조건 비교에서 무조건 True로 처리됩니다.
        /// </summary>
        public const string c_True = "${TRUE}";
        /// <summary>
        /// 암호1 설정 입니다.
        /// </summary>
        public const string c_SetPassword1 = "${SETPASSWORD1}";
        /// <summary>
        /// 암호1 설정 확인 입니다.
        /// </summary>
        public const string c_SetPassword1Confirm = "${SETPASSWORD1CONFIRM}";
        /// <summary>
        /// 암호2설정 입니다.
        /// </summary>
        public const string c_SetPassword2 = "${SETPASSWORD2}";
        /// <summary>
        /// 암호2설정 확인 입니다.
        /// </summary>
        public const string c_SetPassword2Confirm = "${SETPASSWORD2CONFIRM}";
        //mcshin 2010-10-06 [RM-12 Pre-Prompt] Start ----------------------------------------------
        /// <summary>
        /// 이전 프롬프트 입니다.
        /// </summary>
        public const string c_PrePrompt = "${PRE-PROMPT}";
        //mcshin 2010-10-06 [RM-12 Pre-Prompt] End   ----------------------------------------------

        //mjjoe 2009기능고도화 - 예약어관리 CommanClass로 변경 -------------------------------------------------------------------------------<시작>
        /// <summary>
        /// 예약어 리스트를 CommandInfo 클래스 arrayList로 반환합니다.
        /// </summary>
        /// <returns></returns>
        public static List<ReservedString> GetTelnetReservedString()
        {
            List<ReservedString> tCommandList = new List<ReservedString>();
            tCommandList.Add(new ReservedString(TelnetReservedString.c_WAIT, "Prompt결과가 장비에서 반환될 때가지 텔넷작업을 대기합니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_WAIT + "초", "지정한 초만큼 텔넷작업을 대기합니다. 예)" + TelnetReservedString.c_WAIT + "2"));
            //mjjjoe 2009기능고도화 - TIMEOUT, FTPUPLOAD 예약어를 추가합니다. -------------------------------------------------------------<시작>
            tCommandList.Add(new ReservedString(TelnetReservedString.c_TIMEOUT + "초", "지정한 초만큼 명령어 Timeout 시간을 설정과 실패원인을 입력합니다. 예)" + TelnetReservedString.c_TIMEOUT + "4, 패스워드 불일치"));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_FTPUPLOAD, "점검 스케줄링(CONFIG백업)결과를 FTP서버에 백업하는 명령어 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_DBUPLOAD, "점검 스케줄링(CONFIG백업)결과를  DB서버에 백업하는 명령어 입니다."));
            //jwmin 2009기능고도화 - 점검 스케줄링(CONFIG비교) 명령에 사용되는 예약어로 CONFIG비교 작업을 실행하는 명령어 입니다.
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CFGCOMPARE, "점검 스케줄링(CONFIG비교) 명령에 사용되는 예약어로 CONFIG비교 작업을 실행하는 명령어 입니다."));
            //-----------------------------------------------------------------------------------------------------------------------------<끝>
            tCommandList.Add(new ReservedString(TelnetReservedString.c_UserID1, "텔넷 사용자 ID입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_Password1, "텔넷 사용자 암호 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_UserID2, "텔넷 사용자 ID2입니다.(Enable계정)"));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_Password2, "텔넷 사용자 암호2 입니다.(Enable암호)"));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_Space, "텔넷 작업시 예약어가 공백으로 대치 됩니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_Enter, "텔넷 작업시 예약어가 Enter Key로 대치 됩니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_FTPIP, "FTP서버 주소 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_FTPUSER, "FTP사용자 ID입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_FTPPASSEORD, "FTP사용자 암호 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_TFTPIP, "TFTP서버 주소 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_OSFILENAME, "OS파일 이름 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_OSFILESIZE, "OS파일 크기 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_VERSION, "OS 버전 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_DEVICEOSVERSION, "장비의 현재 OS버전 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_EXIT, "작업을 종료 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_NoEnter, "Enter를 장비에 전송하지 않습니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_VARIABLESTRING, "변수의 시작 문자열 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_Seperator, "구분자 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CONFIGFILENAME_IP, "확장자를 제외한 장비 IP주소로 환경파일이름을 생성합니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CONFIGFILENAMEEXT_IP, "확장자가 포함된 장비 IP주소로 환경파일이름을 생성합니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CONFIGFILENAME, "확장자를 제외한 환경파일이름 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CONFIGFILENAMEEXT, "확장자가 포함된 환경파일이름 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CONFIGFILENAME16, "확장자를 제외한 16자리 환경파일이름 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_CONFIGFILENAMEEXT16, "확장자가 포함된 16자리 환경파일이름 입니다."));

            //mjjoe 2009-09-09  조건 비교 뿐만 아니라 점검문자 조건도 무조건 True로 처리되도록 변경
            //tCommandList.Add(new CommandInfo(TelnetReservedString.c_True, "조건 비교에서 무조건 True로 처리됩니다."));  //주석
            tCommandList.Add(new ReservedString(TelnetReservedString.c_True, "점검문자 또는 조건 비교에서 무조건 True로 처리됩니다."));

            tCommandList.Add(new ReservedString(TelnetReservedString.c_SetPassword1, "1차 암호 설정입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_SetPassword1Confirm, "1차 암호 설정 확인 입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_SetPassword2, "2차 암호 설정입니다."));
            tCommandList.Add(new ReservedString(TelnetReservedString.c_SetPassword2Confirm, "2차 암호 설정 확인 입니다."));

            //mcshin 2010-10-06 [RM-12 Pre-Prompt] Start ----------------------------------------------
            tCommandList.Add(new ReservedString(TelnetReservedString.c_PrePrompt, "이전 명령에서 프롬프트 추출하여 프롬프트를 사용합니다."));
            //mcshin 2010-10-06 [RM-12 Pre-Prompt] End   ----------------------------------------------

            return tCommandList;
        }
        //mcshin 2010-10-14 [C01501] AMAS 스크립트 문법 오류 체크 Start --------------------------------------
        /// <summary>
        /// Hash로 된 예약어를 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ReservedString> GetTelnetReservedStringToHash()
        {
            Dictionary<string, ReservedString> tCommandList = new Dictionary<string, ReservedString>();



            tCommandList.Add(TelnetReservedString.c_WAIT, new ReservedString(TelnetReservedString.c_WAIT, "Prompt결과가 장비에서 반환될 때가지 텔넷작업을 대기합니다."));
            tCommandList.Add(TelnetReservedString.c_WAIT + "초", new ReservedString(TelnetReservedString.c_WAIT + "초", "지정한 초만큼 텔넷작업을 대기합니다. 예)" + TelnetReservedString.c_WAIT + "2"));
            //mjjjoe 2009기능고도화 - TIMEOUT, FTPUPLOAD 예약어를 추가합니다. -------------------------------------------------------------<시작>
            tCommandList.Add(TelnetReservedString.c_TIMEOUT + "초", new ReservedString(TelnetReservedString.c_TIMEOUT + "초", "지정한 초만큼 명령어 Timeout 시간을 설정과 실패원인을 입력합니다. 예)" + TelnetReservedString.c_TIMEOUT + "4, 패스워드 불일치"));
            tCommandList.Add(TelnetReservedString.c_FTPUPLOAD, new ReservedString(TelnetReservedString.c_FTPUPLOAD, "점검 스케줄링(CONFIG백업)결과를 FTP서버에 백업하는 명령어 입니다."));
            tCommandList.Add(TelnetReservedString.c_DBUPLOAD, new ReservedString(TelnetReservedString.c_DBUPLOAD, "점검 스케줄링(CONFIG백업)결과를  DB서버에 백업하는 명령어 입니다."));
            //jwmin 2009기능고도화 - 점검 스케줄링(CONFIG비교) 명령에 사용되는 예약어로 CONFIG비교 작업을 실행하는 명령어 입니다.
            tCommandList.Add(TelnetReservedString.c_CFGCOMPARE, new ReservedString(TelnetReservedString.c_CFGCOMPARE, "점검 스케줄링(CONFIG비교) 명령에 사용되는 예약어로 CONFIG비교 작업을 실행하는 명령어 입니다."));
            //-----------------------------------------------------------------------------------------------------------------------------<끝>
            tCommandList.Add(TelnetReservedString.c_UserID1, new ReservedString(TelnetReservedString.c_UserID1, "텔넷 사용자 ID입니다."));
            tCommandList.Add(TelnetReservedString.c_Password1, new ReservedString(TelnetReservedString.c_Password1, "텔넷 사용자 암호 입니다."));
            tCommandList.Add(TelnetReservedString.c_UserID2, new ReservedString(TelnetReservedString.c_UserID2, "텔넷 사용자 ID2입니다.(Enable계정)"));
            tCommandList.Add(TelnetReservedString.c_Password2, new ReservedString(TelnetReservedString.c_Password2, "텔넷 사용자 암호2 입니다.(Enable암호)"));
            tCommandList.Add(TelnetReservedString.c_Space, new ReservedString(TelnetReservedString.c_Space, "텔넷 작업시 예약어가 공백으로 대치 됩니다."));
            tCommandList.Add(TelnetReservedString.c_Enter, new ReservedString(TelnetReservedString.c_Enter, "텔넷 작업시 예약어가 Enter Key로 대치 됩니다."));
            tCommandList.Add(TelnetReservedString.c_FTPIP, new ReservedString(TelnetReservedString.c_FTPIP, "FTP서버 주소 입니다."));
            tCommandList.Add(TelnetReservedString.c_FTPUSER, new ReservedString(TelnetReservedString.c_FTPUSER, "FTP사용자 ID입니다."));
            tCommandList.Add(TelnetReservedString.c_FTPPASSEORD, new ReservedString(TelnetReservedString.c_FTPPASSEORD, "FTP사용자 암호 입니다."));
            tCommandList.Add(TelnetReservedString.c_TFTPIP, new ReservedString(TelnetReservedString.c_TFTPIP, "TFTP서버 주소 입니다."));
            tCommandList.Add(TelnetReservedString.c_OSFILENAME, new ReservedString(TelnetReservedString.c_OSFILENAME, "OS파일 이름 입니다."));
            tCommandList.Add(TelnetReservedString.c_OSFILESIZE, new ReservedString(TelnetReservedString.c_OSFILESIZE, "OS파일 크기 입니다."));
            tCommandList.Add(TelnetReservedString.c_VERSION, new ReservedString(TelnetReservedString.c_VERSION, "OS 버전 입니다."));
            tCommandList.Add(TelnetReservedString.c_DEVICEOSVERSION, new ReservedString(TelnetReservedString.c_DEVICEOSVERSION, "장비의 현재 OS버전 입니다."));
            tCommandList.Add(TelnetReservedString.c_EXIT, new ReservedString(TelnetReservedString.c_EXIT, "작업을 종료 입니다."));
            tCommandList.Add(TelnetReservedString.c_NoEnter, new ReservedString(TelnetReservedString.c_NoEnter, "Enter를 장비에 전송하지 않습니다."));
            tCommandList.Add(TelnetReservedString.c_VARIABLESTRING, new ReservedString(TelnetReservedString.c_VARIABLESTRING, "변수의 시작 문자열 입니다."));
            tCommandList.Add(TelnetReservedString.c_Seperator, new ReservedString(TelnetReservedString.c_Seperator, "구분자 입니다."));
            tCommandList.Add(TelnetReservedString.c_CONFIGFILENAME_IP, new ReservedString(TelnetReservedString.c_CONFIGFILENAME_IP, "확장자를 제외한 장비 IP주소로 환경파일이름을 생성합니다."));
            tCommandList.Add(TelnetReservedString.c_CONFIGFILENAMEEXT_IP, new ReservedString(TelnetReservedString.c_CONFIGFILENAMEEXT_IP, "확장자가 포함된 장비 IP주소로 환경파일이름을 생성합니다."));
            tCommandList.Add(TelnetReservedString.c_CONFIGFILENAME, new ReservedString(TelnetReservedString.c_CONFIGFILENAME, "확장자를 제외한 환경파일이름 입니다."));
            tCommandList.Add(TelnetReservedString.c_CONFIGFILENAMEEXT, new ReservedString(TelnetReservedString.c_CONFIGFILENAMEEXT, "확장자가 포함된 환경파일이름 입니다."));
            tCommandList.Add(TelnetReservedString.c_CONFIGFILENAME16, new ReservedString(TelnetReservedString.c_CONFIGFILENAME16, "확장자를 제외한 16자리 환경파일이름 입니다."));
            tCommandList.Add(TelnetReservedString.c_CONFIGFILENAMEEXT16, new ReservedString(TelnetReservedString.c_CONFIGFILENAMEEXT16, "확장자가 포함된 16자리 환경파일이름 입니다."));

            //mjjoe 2009-09-09  조건 비교 뿐만 아니라 점검문자 조건도 무조건 True로 처리되도록 변경
            //tCommandList.Add(new CommandInfo(TelnetReservedString.c_True, "조건 비교에서 무조건 True로 처리됩니다."));  //주석
            tCommandList.Add(TelnetReservedString.c_True, new ReservedString(TelnetReservedString.c_True, "점검문자 또는 조건 비교에서 무조건 True로 처리됩니다."));

            tCommandList.Add(TelnetReservedString.c_SetPassword1, new ReservedString(TelnetReservedString.c_SetPassword1, "1차 암호 설정입니다."));
            tCommandList.Add(TelnetReservedString.c_SetPassword1Confirm, new ReservedString(TelnetReservedString.c_SetPassword1Confirm, "1차 암호 설정 확인 입니다."));
            tCommandList.Add(TelnetReservedString.c_SetPassword2, new ReservedString(TelnetReservedString.c_SetPassword2, "2차 암호 설정입니다."));
            tCommandList.Add(TelnetReservedString.c_SetPassword2Confirm, new ReservedString(TelnetReservedString.c_SetPassword2Confirm, "2차 암호 설정 확인 입니다."));

            //mcshin 2010-10-06 [RM-12 Pre-Prompt] Start ----------------------------------------------
            tCommandList.Add(TelnetReservedString.c_PrePrompt, new ReservedString(TelnetReservedString.c_PrePrompt, "이전 명령에서 프롬프트 추출하여 프롬프트를 사용합니다."));
            //mcshin 2010-10-06 [RM-12 Pre-Prompt] End   ----------------------------------------------

            return tCommandList;
        }
        //mcshin 2010-10-14 [C01501] AMAS 스크립트 문법 오류 체크 End   --------------------------------------

        public static bool isPasswordReservedString(string aCheckCmd)
        {
            if (aCheckCmd.Equals(TelnetReservedString.c_Password1))
                return true;

            if (aCheckCmd.Equals(TelnetReservedString.c_Password2))
                return true;

            return false;
        }

        public static Boolean isTelnetReservedString(string aCheckCmd)
        {
            if (aCheckCmd.IndexOf("${") >= 0)
            {
                return true;
            }

            string tCheckStr = aCheckCmd.ToLower();

            if (tCheckStr.Equals("en") || tCheckStr.Equals("enable"))
            {
                return true;
            }

            return false;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------<끝>
    }
    #endregion //예약 문자열 정의 부분 입니다 --------------------------------------------------
}
