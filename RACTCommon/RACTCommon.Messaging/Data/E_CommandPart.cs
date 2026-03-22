using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public enum E_CommandPart : int
    {
        // 2013-01-11 - shinyn - Command Part 정보입니다.
        /// <summary>
        /// 명령 파트가 아닙니다.
        /// </summary>
        None = -1,
        /// <summary>
        /// Snmp점검 입니다.(0)
        /// </summary>
        SnmpCheck = 0,
        /// <summary>
        /// Telnet점검 입니다.(1)
        /// </summary>
        TelnetCheck = 1,
        /// <summary>
        /// Snmp설정 입니다.(2)
        /// </summary>
        SnmpSetting = 2,
        /// <summary>
        /// Telnet설정 입니다.(3)
        /// </summary>
        TelnetSetting = 3,
        /// <summary>
        /// Telnet임계치 명령 입니다.(8)
        /// </summary>
        TelnetThreshold = 8,
        /// <summary>
        /// Snmp임계치 명령 입니다.(9)
        /// </summary>
        SnmpThreshold = 9,
        /// <summary>
        /// Ping명령 입니다.(15)
        /// </summary>
        Ping = 15,
        /// <summary>
        /// 장비 온도 점검 명령 입니다.(21)
        /// </summary>
        SystemTemperature = 21,
        /// <summary>
        /// Cup 5초 사용률 입니다.(22)
        /// </summary>
        CpuStatus5Sec = 22,
        /// <summary>
        /// Cpu 1분 사용률 입니다.(23)
        /// </summary>
        CpuStatus1Minute = 23,
        /// <summary>
        /// Cup 10분 사용률 입니다.(24)
        /// </summary>
        CpuStatus10Minute = 24,
        /// <summary>
        /// 전체 메모리 점검 입니다.(25)
        /// </summary>
        MemoryTotal = 25,
        /// <summary>
        /// 현재 사용 메모리 점검 입니다.(26)
        /// </summary>
        MemoryUse = 26,
        /// <summary>
        /// 현재 사용가능 메모리 점검 입니다.(27)
        /// </summary>
        MemoryFree = 27,
        /// <summary>
        /// DHCP IP Pool점검 명령 입니다.(28)
        /// </summary>
        IPPoolCheck = 28,
        /// <summary>
        /// 현재 Config점검 입니다.(55)
        /// </summary>
        //RunningConfigCheck = 55,           //2010-08-09 hanjiyeon 주석 - 해당 메뉴 사용안함
        /// <summary>
        /// 암호 설정 명령 입니다.(59)
        /// </summary>
        PasswordSetting = 59,
        /// <summary>
        /// Config저장 명령 입니다.(62)
        /// </summary>
        //ConfigSave = 62,        //2010-08-09 hanjiyeon 주석 - 해당 메뉴 사용안함
        /// <summary>
        /// 재시작 명령 입니다.(63)
        /// </summary>
        Rebooting = 63,
        /// <summary>
        /// TCP Dump명령 입니다.(64)
        /// </summary>
        TCPDump = 64,
        /// <summary>
        /// 기본 장비 접속 명령 입니다.(65)
        /// </summary>
        DefaultConnectCommand = 65,
        /// <summary>
        /// OSUpgrade버젼 확인 입니다.(101)
        /// </summary>
        OSUVersionCheck = 101,
        /// <summary>
        /// OSUpgrade다운 로드 입니다.(102)
        /// </summary>
        OSUDownloadCheck = 102,
        /// <summary>
        /// OSUpgrade재시작 입니다.(103)
        /// </summary>
        OSURebootCheck = 103,
        /// <summary>
        /// OSUpgrade업그레이드 검수 입니다.(104)
        /// </summary>
        OSUUpgradeCheck = 104,
        //2007-12-06 hanjiyeon 추가 tasknet 11 수정
        /// <summary>
        /// OS업그레이드 명령의 Config 백업 단계입니다.(105)
        /// </summary>
        OSUConfigBackup = 105,
        /// <summary>
        /// Config백업/복원 백업 입니다.(201)
        /// </summary>
        ConfigBRBackup = 201,
        /// <summary>
        /// Config백업/복원 복원 입니다.(202)
        /// </summary>
        ConfigBRRestore = 202,
        /// <summary>
        /// Config백업/복원 재시작 입니다.(203)
        /// </summary>
        ConfigBRReboot = 203,
        //2008-09-25 hanjiyeon 추가 - fact2008고도화 : vdsl bit-load map 기능 추가.
        /// <summary>
        /// VDSL Bit-Load Map 입니다. (204)
        /// </summary>
        //	VDSLBitLoadMap = 204,        //2010-08-09 hanjiyeon 주석 - 해당 메뉴 사용안함
        //2010-07-06 hanjiyeon 추가 [2010고도화II] - Config 저장
        /// <summary>
        /// Telnet Config 점검 입니다.(403)
        /// </summary>
        TELNETConfigCheck = 403,
        /// <summary>
        /// Snmp Config 점검 입니다.(404)
        /// </summary>
        SNMPConfigCheck = 404,

        //2009-10-16 hanjiyeon : 2009고도화 (명령분류 추가) start -
        /// <summary>
        /// 표준 Config 비교 입니다. (300)
        /// </summary>
        ConfigCompare = 300,
        //2013-01-02 hanjiyeon 추가 - 표준Config 비교 조치
        /// <summary>
        /// 표준Config 비교 조치 입니다. (301)
        /// </summary>
        ConfigCompareAction = 301,
        //2009-10-16 Config 저장 추가
        /// <summary>
        /// Config 다운로드 (Text) 입니다. (400)
        /// </summary>
        ConfigSaveToText = 400,
        /// <summary>
        /// Telnet 버전점검 입니다.(401)
        /// </summary>
        TELNETVersionCheck = 401,
        //2010-07-10 hanjiyeon 추가 [2010고도화II] - 점검자동화 버전점검 Telnet/Snmp 모두 허용
        /// <summary>
        /// Snmp 버전점검 입니다. (402)
        /// </summary>
        SNMPVersionCheck = 402,
        //2009-10-16 hanjiyeon : 2009고도화 (명령분류 추가) -end      
        //2010-07-20 hanjiyeon 추가 [2010고도화II] 
        /// <summary>
        /// Telnet G/E Pon 점검 입니다.
        /// </summary>
        //  TELNETGEPONCheck = 501,
        /// <summary>
        /// Snmp G/E Pon 점검 입니다.
        /// </summary>
        SNMPGEPON = 502,
        /// <summary>
        /// SNMP G/E-PON ONUType 점검 입니다. = 505
        /// </summary>
        SNMPGEPONONUTypeCheck = 505,
        //2010-08-24 hanjiyeon 추가 [2010고도화 2차]
        /// <summary>
        /// G/E-PON FW점검(점검자동화) 입니다. = 506
        /// </summary>
        SNMPGEPONFWCheckSchedule = 506,
        /// <summary>
        /// G/E-PON OLT 버전점검 점검(수동) 입니다. = 507
        /// </summary>
        //SNMPGEPONOLTVersionCheck = 507,
        /// <summary>
        /// G/E-PON OLT FW점검 (수동) 입니다. = 508
        /// </summary>
        // SNMPGEPONFWCheck = 508,
        /// <summary>
        /// G/E-PON OLT FW다운로드 (수동)입니다. = 509
        /// </summary>
        //SNMPGEPONFWDownLoad = 509,

        /// <summary>
        /// G/E-PON IPTV 채널 검색(찾기) 입니다. = 600
        /// </summary>
        SNMPGPONIPTVChannelFind = 600,
        /// <summary>
        /// G/E-PON IPTV 채널 조회 입니다. = 601
        /// </summary>
        SNMPGPONIPTVChannelSearch = 601,
        /// <summary>
        /// G/E-PON IPTV 채널 추가 입니다. = 602
        /// </summary>
        SNMPGPONIPTVChannelAdd = 602,
        /// <summary>
        /// G/E-PON IPTV 채널 삭제 입니다. = 603
        /// </summary>
        SNMPGPONIPTVChannelRemove = 603,
        //2010-11-15 hanjiyeon
        /// <summary>
        /// G/E-PON IPTV 채널 점검 (점검자동화) 입니다. = 604
        /// </summary>
        SNMPGPONIPTVChannelCheck = 604,
        //2011-08-09 hanjiyeon [2011고도화] R11.OS업그레이드 모니터링
        /// <summary>
        /// OS업그레이드 모니터링 입니다. = 700
        /// </summary>
        OSUpgradeMonitoring = 700,
        /// <summary>
        /// 성능모니터링 입니다. = 701
        /// </summary>
        Monitoring = 701,
        /// <summary>
        /// 포트형상관리 입니다. = 702
        /// </summary>
        PortConfiguration = 702,
    }
}
