REM ##############################################################################
REM #일반접속 파라미터: TACTServerIP Account Pwd DeviceIP
REM #TACTOneTerminal 118.217.79.41 daims ekdlawm12#$ 100.77.35.131
REM #RCCS접속(17고도화) 파라미터: TACTServerIP Account Pwd DeviceIP 1 RCCSDeviceIP RCCSDevicePort
REM #RPCS-유선접속(18고도화) 파라미터: TACTServerIP Account Pwd DeviceIP 2 RPCSDeviceIP RPCSDevicePort(u-art)
REM #RPCS-LTE접속(18고도화) 파라미터: TACTServerIP Account Pwd DeviceIP 3
REM ##############################################################################

REM #기가ONU / H5224G (4069)
TACTOneTerminal 118.217.79.41 daims3 ekdlawm12#$ 100.94.144.39

REM ------------------------------------------------------------------------------------
REM  ■ TACT OneTerminal 접속 파라미터 (Last updated: 2019.03.12)
REM ------------------------------------------------------------------------------------
REM (기존) 일반  telnet 접속 로그인
REM 1.TACT서버IP 2.유저계정 3.계정Password 4.최종접속장비IP

REM (기존2017) RCCS 접속 로그인 
REM 1.TACT서버IP 2.유저계정 3.계정Password 4.최종접속장비IP 5.접속구분(1) 6.해당RCCS장비IP 7.해당RCCS장비의Port번호
REM TACTOneTerminal.exe 118.217.79.41 jincoms3 DASAN_HRKIM 121.124.245.139 1 58.121.181.57 1001

REM (신규2018) RPCS 접속 로그인 유선접속( 콘솔(Uart) 접속 )
REM 1.TACT서버IP 2.유저계정 3.계정Password 4.최종접속장비IP 5.접속구분(2) 6.해당RPCS장비IP 7.해당RPCS장비의Port번호
REM TACTOneTerminal.exe 118.217.79.41 da**** ekd***** 221.140.51.133 2 221.140.51.133 1001

REM (신규2018) RPCS 접속 로그인 무선접속( SSH터널링 접속 )
REM 1.TACT서버IP 2.유저계정 3.계정Password 4.최종접속장비IP 5.접속구분(3)
REM TACTOneTerminal.exe 118.217.79.41 da**** ekd***** 221.140.51.133 3

REM <주석>
REM 1.TACT서버IP : 기존에 설정하고 있던 TACT서버IP를 설정하면 됩니다.(현재 118.217.79.41 )
REM 2.유저계정 : User Account
REM 3.계정Password : 암호화 안된 Password
REM 4.최종접속장비IP : 일반의경우 접속장비IP, RCCS 로그인의 경우 최종접속장비IP
REM 5.접속구분 : RCCS 로그인의 경우 1 고정
REM 6.해당RCCS장비IP : 경유 RCCS장비의 IP
REM 7.해당RCCS장비의Port번호 : RCCS장비에 접속장비와 연결되어져 있는 Port번호
