---
name: gstack
description: garrytan/gstack 방법론에 기반한 Think → Plan → Build → Review → Test → Ship → Reflect 개발 사이클 강제 적용 스킬
---
# gstack Skill

이 스킬은 Antigravity 에이전트가 코드를 작성하기 전/후에 겪어야 하는 체계적이고 치밀한 개발 프로세스를 (garrytan의 gstack 패턴에 따라) 프로젝트에 적용합니다.

## 핵심 스프린트 7단계 (The Sprint Process)

1. **`/office-hours` (Think)**
   - 역할: YC Office Hours (문제 재정의자)
   - 행동: 유저의 요구사항에 대해 곧바로 코딩하지 않고, 근본적인 문제를 다시 짚어보며 3가지 대안을 제시합니다.

2. **`/plan-ceo-review` (Plan - Scope)**
   - 역할: CEO / Founder
   - 행동: 제안된 기획을 바탕으로 "10성급(10-star) 제품"이 되기 위한 스코프 확장을 고민하거나 불필요한 스코프를 축소합니다.

3. **`/plan-eng-review` (Plan - Architecture)**
   - 역할: Eng Manager
   - 행동: 아키텍처, 데이터 흐름, 엣지 케이스, 테스트 계획을 확정합니다. (이때 `implementation_plan.md`를 구체화합니다.)

4. **`/review` (Review)**
   - 역할: Staff Engineer
   - 행동: 작성된 코드가 프로덕션 환경에서 발생시킬 수 있는 숨은 버그나 예외 처리를 철저히 리뷰하고 수정합니다.

5. **`/qa` (Test)**
   - 역할: QA Lead
   - 행동: 코드에 대한 회귀 테스트(Regression Test)가 존재하는지 확인하고, QA 절차(수동/자동)를 주도합니다.

6. **`/ship` (Ship)**
   - 역할: Release Engineer
   - 행동: 테스트 통과 및 커버리지를 확인하고 PR/Commit을 준비합니다.

7. **`/document-release` (Reflect)**
   - 역할: Technical Writer
   - 행동: 기능 구현에 맞춰 README, ARCHITECTURE, CLAUDE.md 등 모든 문서를 최신화하여 Doc Drift를 방지합니다.

## 적용 방법 (How to use)

- 사용자가 특정 슬래시 명령어(예: `/review` 또는 `/plan-eng-review`)를 호출하면, 위 정의된 페르소나와 검증 절차에 따라 철저하게 응답하고 코드를 수정합니다.
- 복잡한 기능을 구현할 때, 이 7단계를 순서대로 밟아 완성도와 안정성을 극대화하십시오.
