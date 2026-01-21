REM 1)직접 로그인 
REM 파라미터: ①TACT서버IP ②사용자ID ③사용자PW ④장비IP 
REM TACTOneTerminal 118.217.79.41 daims3 DASAN_HRKIM 175.118.128.246
REM TACTOneTerminal 118.217.79.41 daims3 ekdlawm12#$ 100.81.245.141
TACTOneTerminal 118.217.79.41 daims8 ekdlawm12#$ 100.72.243.13

REM 2)RCCS경유 로그인 <2017고도화: RCCS 연동 >
REM  파라미터: ①TACT서버IP ②사용자ID ③사용자PW ④장비IP ⑤접속구분=2 ⑥경유RCCS장비IP ⑦경유RCCS장비포트
REM TACTOneTerminal 118.217.79.41 daims3 DASAN_HRKIM 121.124.245.139 1 58.121.181.57 1001
REM TACTOneTerminal 118.217.79.41 daims3 DASAN_HRKIM 1.235.216.39 1 116.120.83.37 1005

REM 3)c-RPCS 무선 로그인 <2018고도화: c-RPCS 원격접속기능,오정우M/다산>
REM  파라미터: ①TACT서버IP ②사용자ID ③사용자PW ④RPCS장비IP ⑤접속구분=3
REM TACTOneTerminal 118.217.79.41 유저계정 유저비밀번호 rpcs 장비IP 3