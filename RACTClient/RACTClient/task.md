# 리팩토링 작업 완료: TerminalPanel.cs

`TerminalPanel.cs`의 구조를 개선하여 가독성을 높이고 중복 로직을 제거하며, 비동기 처리를 안정화했습니다.

## 할 일 목록
- [x] 터미널 생성 및 추가 로직 통합 (AddTerminal, MakeEmulator 등)
- [x] 명령 및 스크립트 전송 로직의 중복 제거 (ExecuteActionOnTarget 도입)
- [x] 컨텍스트 메뉴 및 UI 이벤트 핸들러 정리
- [x] 비동기 메서드(`async Task` 활용 및 예외 처리) 개선
- [x] 불필요한 주석 및 레거시 코드 제거
- [x] 리소스 정리 및 메모리 누수 방지 (역순 순회 종료 처리 등)
