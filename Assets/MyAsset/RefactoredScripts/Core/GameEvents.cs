using System;
using UnityEngine;

namespace CardDefense.Core.Events
{
    // ==========================================
    // 여기서 게임에서 일어나는 모든 상태 변경을 정의합니다.
    // 기존 ActionQueue가 하던 일을 '이벤트 발생'으로 바꿉니다.
    // ==========================================

    /// <summary>
    /// 대상이 데미지를 입었을 때 발생하는 이벤트
    /// 이걸 UI 시스템(플로팅 텍스트), VFX 시스템(피격 파티클)이 각자 듣고 처리.
    /// 로직 멈춤(await) 방지.
    /// </summary>
    public struct EntityTakeDamageEvent : IGameEvent
    {
        public GameObject Target; // 누가 맞았는지
        public int DamageAmount;  // 얼마의 데미지인지
        public Vector3 HitPosition; // 어디서 맞았는지(파티클 생성 위치)
    }

    /// <summary>
    /// 대상의 체력이 0이 되어 사망했을 때 발생하는 이벤트
    /// 여기서 웨이브 컨트롤러나 킬 카운트 UI가 수신 대기.
    /// </summary>
    public struct EntityDeathEvent : IGameEvent
    {
        public GameObject DeadEntity;
        public bool IsEnemy;
    }

    /// <summary>
    /// 손에서 카드를 사용했을 때 발생하는 이벤트 (게임 행동의 시작점)
    /// </summary>
    public struct CardUsedEvent : IGameEvent
    {
        public string CardID;      // 카드 식별자
        public Vector3 CastPosition; // 시전 위치
        public CardDefense.Cards.RefactoredCardData PlayedCard; // 사용된 카드 데이터 (단일/복합 이펙트 포함)
    }

    /// <summary>
    /// 카드 이펙트 등에서 카드를 뽑으라고 시스템에 명령하는 이벤트
    /// </summary>
    public struct TriggerDrawCardEvent : IGameEvent
    {
        public int Amount; // 몇 장을 뽑을 것인지
    }
}
