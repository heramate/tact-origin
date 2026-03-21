---
description: Run the gstack sprint process for a new feature or refactor
---
# gstack Sprint Workflow

이 워크플로우는 `gstack`의 7단계 개발 스프린트를 실행합니다. 아래 순서대로 각 단계를 진행하세요.

1. **Think (`/office-hours`)**: 요구사항의 본질을 파악하고 대안 설계 (Design Doc 작성).
2. **Plan Scope (`/plan-ceo-review`)**: 비즈니스 관점의 스코프 검토 및 확정.
3. **Plan Architecture (`/plan-eng-review`)**: 기술적 아키텍처 및 테스트 계획 확정 (`implementation_plan.md` 생성/업데이트).
4. **Review (`/review`)**: 프로덕션 레벨의 버그/엣지 케이스 점검 (Staff Engineer 관점).
5. **Test (`/qa`)**: 회귀 테스트 자동화 및 검증.
6. **Ship (`/ship`)**: 최종 릴리스 검토 및 커버리지 확인.
7. **Reflect (`/document-release`)**: README 및 설계 문서 최신화.

사용자가 슬래시 명령어(`/gstack-sprint`)를 입력하면 위 단계를 안내하고, 첫 단계인 `/office-hours`부터 시작해 의사결정을 유도합니다.
