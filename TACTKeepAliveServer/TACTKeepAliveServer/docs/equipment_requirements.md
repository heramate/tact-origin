# TACT KeepAlive 서버 장비 연동 요건

장비(단말)가 TACT KeepAlive 서버와 실시간 상태 동기화 및 SSH 터널링 원격 제어를 수행하기 위해 갖춰야 할 기술적 요건입니다.

## 1. 네트워크 통신 요건
- **프로토콜**: UDP (User Datagram Protocol)
- **서버 포트**: 기본 `40001` (설정에 따라 변경 가능)
- **통신 방식**: 
    - 장비는 주기적으로 서버에 Keep-Alive 패킷을 송신해야 함.
    - 서버로부터 오는 UDP 응답(Reply)을 수신하고 처리할 수 있어야 함. (고정 IP가 아닌 경우 NAT 세션 유지를 위해 짧은 주기의 송신 필요)

## 2. 데이터 포맷 및 인코딩 요건

### 2.1 Base64 인코딩
- 서버 설정(KeepAliveBase64Encode)에 따라 UDP 페이로드 전체를 **Base64**로 인코딩/디코딩할 수 있어야 함.

### 2.2 메시지 구조 (TLV 방식)
모든 메시지는 `FACT` (4바이트) 프리픽스로 시작하며, 이후 `Type-Length-Value` 형태로 구성됩니다.

| 필드 | 길이 | 설명 |
| :--- | :--- | :--- |
| **Prefix** | 4 Bytes | 고정 문자열 `FACT` (UTF-8) |
| **Type** | 1 Byte | 데이터 항목 구분 아이디 |
| **Length** | 1 Byte | Value의 길이 |
| **Value** | 가변 | 실제 데이터 값 |

### 2.3 주요 TLV 항목 (필수/선택)
| Type | 항목명 | 필수여부 | 설명 |
| :--- | :--- | :---: | :--- |
| 1 | ModelName | 필수 | 장비 모델명 (String) |
| 2 | SerialNumber | 필수 | 장비 일련번호 (String) |
| 3 | USIM | **필수** | 가입자 식별 번호 (Primary Key로 사용) |
| 5 | IPv4Address | 필수 | 장비의 현재 할당된 IP (4 Bytes, Network Byte Order) |
| 8 | SSHTunnelOption | 선택 | 터널 제어 옵션 (1: Open, 255: Close, 0: Ack) |

## 3. SSH 터널링 제어 요건

장비가 원격 제어 기능을 지원하려면 다음 소프트웨어 스택이 필요합니다.

- **SSH Client/Server**: 
    - 서버의 요청(Type 8: Open)을 받으면 서버 주소로 **Reverse SSH Tunneling**을 수행할 수 있어야 함.
- **자동 재연결**: 터널이 비정상 종료되거나 서버로부터 Close 요청을 받으면 즉시 리소스를 해제해야 함.
- **항목 수신 처리**: 서버가 보내는 응답 패킷에 포함된 `SSHServerDomain`, `SSHPort`, `SSHUserID`, `SSHPassword`, `SSHTunnelPort` 정보를 해석하여 접속에 사용해야 함.

## 4. 운영 요건 (Best Practice)
- **전송 주기**: LTE망 환경을 고려하여 보통 1분~5분 주기로 Keep-Alive 전송 권장.
- **타임아웃 처리**: 서버로부터 일정 시간 이상 응답이 없어도 주기적인 전송을 멈추지 않아야 함. (네트워크 복구 시 즉시 인지 가능하도록 함)
