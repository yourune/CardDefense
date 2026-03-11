# 타워디펜스 카드 게임 - 실시간 이벤트 기반 아키텍처 리팩토링 플랜

## 🛠 아키텍처 설계 원칙
1. **로직과 연출의 완벽한 분리**: 데미지, 사망 등 모든 로직은 즉시(Instant) 계산되며, 코루틴으로 인한 대기가 발생하지 않아야 함. 시각 효과(VFX)와 애니메이션은 Event를 수신하여 비동기(DOTween/UniTask)로 재생.
2. **이벤트 버스 패턴 (Event-Driven)**: `ActionSystem`과 같은 글로벌 큐 방식 폐기. `EventBus`를 통해 시스템 간 느슨한 결합 달성.
3. **최적화 (오브젝트 풀링 & 물리)**: 다수의 Zone과 수백 명의 적을 위해 충돌(Zone) 처리는 큐 대기열이 아닌 `Physics.OverlapSphere`, 타겟 캐싱을 활용. 파티클 및 적 객체는 무조건 풀링.
4. **점진적 교체 (안전장치)**: 기존 에셋(JMO, BG 등)과의 충돌 방지. 기존 스크립트 수정 금지(필요시 `_Name`으로 백업 후 새로 작성). 새로운 스크립트는 `RefactoredScripts` 폴더 내 직무별로 패키징.

---

## 📅 리팩토링 진행 단계 (Phase)

### [✔️] Phase 1: 기반 시스템 구축 (Foundation)
- **완료**: `RefactoredScripts` 폴더 구조 생성 (`Core`, `Entities`, `Systems`, `Cards`, `Visuals`)
- **완료**: 코어 이벤트 버스(`EventBus`, `GameEvents`) 및 범용 오브젝트 풀 매니저 기본형 생성.

### [✔️] Phase 2: 코어 엔티티 로직 (Entities & Combat)
- **완료**: 로직 전용 컴포넌트 개발 (`HealthComponent`). 기존 `CombatantView` 등을 대체할 새로운 클래스 `EnemyEntity`, `CastleEntity` 작성 완료.
- **완료**: `EnemyEntity`에 기존 `EnemyData` ScriptableObject 연동 완료.
- **목표 (달성)**: 데미지를 입고, HP가 깎이고, 죽는 처리(TakeDamage)가 1프레임 즉시 일어나고 `EventBus`를 통해 이벤트만 발송하도록 수정.

### [✔️] Phase 3: 장판 및 상호작용 로직 (Zones & Physics Intersect)
- **완료**: 광역 공격 및 장판 생성을 위한 `AoECombatHelper` 와 `PhysicalTickZone` 구현.
- **목표 (달성)**: `ActionQueue`를 거치지 않고, `Physics.OverlapSphereNonAlloc`을 통해 메모리 가비지 없이 범위 안의 수백 명의 적을 한 프레임에 동시 타격(Instant / Tick-based Damage)하는 시스템 구축.

### [✔️] Phase 4: 카드 시스템 개편 (Realtime Hand System)
- **완료**: Card 사용을 Event 기반으로 분리, `RefactoredHandSystem`, `RefactoredCardView` 개편. (기존 ActionQueue 제거)

### [✔️] Phase 5: 카드 효과 스크립터블 오브젝트 통합 구조 (Effect SO System)
- **완료**: `CardData`가 다수의 "즉발/비동기 Effect"(`IEngineEffect`)들을 소유하는 패턴 구축
- **완료**: DOTween의 파괴(Destroy) 널레퍼런스 경고 수정 (DOKill 추가 및 OnComplete에서의 안전한 파괴 구문 적용)

### [✔️] Phase 6: 패 시스템 개편 (Grid Hand, Deck, Discard Pile)
- **완료**: 스플라인(Spline) 곡선 기반의 겹치는 패를 폐기하고, **5x2 격자(Grid)** 방식의 카드 덱/드로우/버리기 시스템 구축.
- **완료**: 인스펙터에서 지정한 좌표(`Card Slots`)로 정확히 날아가 꽂히며, 중간의 카드를 사용시 남은 카드가 앞으로 당겨짐.
- **완료**: 스폰 시 덱(DrawPile) 위치에서 생성되어 날아오고, 버려질 땐 무덤(DiscardPile) 위치로 날아가 축소되며 파괴됨.

## 📅 향후 리팩토링 진행 단계 (Phase 7 ~ 11)

### [ ] Phase 7: 코어 자원 및 보상 시스템 (Resource & Reward)
- **대상 스크립트**: `ManaSystem`, `RewardSystem`, `ManaUI`, `RewardUI`
- **목표**: 대기열(`GameAction`)을 통하지 않고 EventBus를 활용해 자원의 즉각적인 변동 처리.
- **세부 작업**:
  1. 마나 변동 시스템 이벤트화 (마나 소모/회복 즉시 UI 반영)
  2. 적 사망 이벤트(`EntityDeathEvent`) 연동 (보상 드롭 -> 골드, 경험치 획득 즉시 연산)

### [ ] Phase 8: 적군 스폰 및 웨이브 시스템 (Wave & Spawning)
- **대상 스크립트**: `WaveSystem`, `EnemySystem`, `EnemyViewCreator`
- **목표**: 렉 방지를 위한 대규모 적 객체 풀링(Object Pooling) 연동 및 웨이브 매니저 재구축.
- **세부 작업**:
  1. `WaveSystem`을 코루틴이 아닌 자체 타이머 기반 스폰 로직으로 변경.
  2. 스폰된 적 처치 시 킬 카운트 연동 (다음 웨이브 진행).
  3. `CastleEntity` 도달 시 공격 로직을 물리 충돌(Trigger) 기반으로 변경 (`AttackCastleGA` 대체).

### [ ] Phase 9: 상호작용 및 UI/UX 시스템 개편 (Interaction & Visual)
- **대상 스크립트**: `CardViewHoverSystem`, `ChangeCursorSystem`, `Interactions`
- **목표**: 카드 드래그 시 필요한 정보 노출 및 타겟팅 UX 직관화.
- **세부 작업**:
  1. 카드를 쥐었을 때 커서 변경 및 범위(AoE Indicator) 포지셔닝 로직 분리.
  2. 카드 Hover 애니메이션(DOTween) 통합 적용.

### [ ] Phase 10: 고급 장판 및 체인 시스템 (Advanced Zones & Chains)
- **대상 스크립트**: `ZoneManager`, 고급 Effect 스크립트들 (Shockwave 등)
- **목표**: 장판 소멸, 변화(Wet -> Electric) 및 이동하는 범위 공격 개편.
- **세부 작업**:
  1. 장판 레이어(Layer) 마스킹을 통한 서로 다른 장판끼리의 상호작용 로직(OverlapSphere) 추가.
  2. 이동하는 이펙트(웨이브 공격, 총알 등)를 위한 `Tick` 베이스 투사체 클래스 작성.

### [ ] Phase 11: 잔여 스크립트 클리닝 및 통합 (Cleanup & Polishing)
- **대상**: 사용하지 않게 된 기존 `ActionSystem`, `GameAction` 클래스 17종 전면 폐기 가능 상태 확인.
- **목표**: 프로젝트 컴파일 의존성에서 옛 구조를 완전히 도려내고, 최종 밸런스 테스트.

---

## 📝 특이 사항 (기록용)
- 현재 Hierarchy 구조: `SETUP`, `UI`, `VIEWS`, `SYSTEMS`, `CREATORS`
- 애니메이션은 최대한 **DOTween**을 사용할 것. 비동기 연결이 꼭 필요한 곳은 **UniTask** 사용.
