using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardDefense.Core.Effects
{
    /// <summary>
    /// 카드나 장판이 실행할 수 있는 "효과(Effect)"의 최상위 인터페이스.
    /// 기존의 GameAction / Effect 클래스를 완전 대체합니다.
    /// 코루틴(Delay)을 쓰지 않고 즉시(Instant) 실행되며, 결과만 EventBus로 뿌립니다.
    /// </summary>
    public interface IEngineEffect
    {
        // Vector3.zero, null 등 효과 성격에 따라 사용하지 않는 파라미터는 무시합니다.
        void Execute(Vector3 targetPosition, GameObject targetEntity = null);
    }
}
