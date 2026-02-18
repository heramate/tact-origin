# TACT KeepAlive 서버 통신 흐름도

리팩토링된 .NET 9 시스템의 컴포넌트 간 상세 상호작용 및 통신 흐름입니다.

## 1. Keep-Alive 수신 및 DB 업데이트 흐름

장비(Equipment)로부터 전달된 상태 메시지를 비동기적으로 처리하여 데이터베이스를 최신 상태로 유지하는 주 흐름입니다.

```mermaid
sequenceDiagram
    autonumber
    participant D as 장비 (Equipment)
    participant R as KeepAliveReceiverService
    participant C as Memory Channel (System.Threading.Channels)
    participant B as BatchDbUpdateService
    participant DB as SQL Server (Dapper)

    Note over D, R: UDP (Base64 + TLV)
    D->>R: 1. Keep-Alive 패킷 송신
    
    rect rgb(240, 240, 240)
    Note over R: 고성능 파싱 단계
    R->>R: 2. Base64 디코딩 (Span<byte> 사용)
    R->>R: 3. TLV 데이터 추출 (KeepAliveMsg)
    end

    R->>C: 4. 처리된 메시지 Push
    C-->>B: 5. 메시지 소비 (Consume)
    
    rect rgb(230, 255, 230)
    Note over B, DB: 배치 처리 및 DB 동기화
    B->>DB: 6. P_LTE_KeepAlive_UPDATE_LTE_NE 호출
    DB-->>B: 7. 장비 정보 업데이트 결과 응답
    end
    
    B->>B: 8. 성공 로깅 (Serilog)
```

## 2. SSH 터널 제어 흐름 (요청 시)

데몬이나 관리자의 요청에 의해 장비의 SSH 터널을 열거나 닫는 제어 흐름입니다.

```mermaid
sequenceDiagram
    autonumber
    participant Adm as 데몬 / 관리자
    participant CP as DaemonCommProcess
    participant TM as SSHTunnelManager
    participant KM as KeepAliveManager
    participant R as KeepAliveSender (Receiver와 공유)
    participant D as 장비 (Equipment)

    Adm->>CP: 1. 터널 생성 요청 (TCP/API)
    CP->>TM: 2. 가용 포트 확인 및 상태 전환
    TM->>KM: 3. 장비의 최신 수신 주소(LteSession) 조회
    KM-->>TM: 4. 최신 KeepAlive 정보 반환
    
    rect rgb(255, 240, 240)
    Note over TM, D: 제어 패킷 전송
    TM->>R: 5. 응답(Reply) 패킷 생성 및 전송 요청
    R-->>D: 6. Open/Close 옵션 포함된 UDP 송신
    end

    D->>D: 7. 장비 내부 SSH Tunnel 프로세스 기동
    D->>TM: 8. (다음 Keep-Alive 주기) 터널 상태 보고
```

## 3. 주요 기술적 특징

- **비동기 큐잉**: 수신부(Receiver)와 처리부(BatchUpdate)를 `Channel`로 분리하여 대량의 패킷 유입 시에도 응답성을 유지합니다.
- **메모리 최적화**: 모든 단계에서 `Span<byte>`를 사용하여 불필요한 복사와 문자열 할당을 방지합니다.
- **배치 DB 작업**: 개별 업데이트 대신 일정한 주기로 DB를 업데이트하여 데이터베이스 서버의 부하를 줄입니다.
