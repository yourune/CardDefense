using System;
using UnityEngine;
using CardDefense.Core.Events;

namespace CardDefense.Entities
{
    /// <summary>
    /// 아군의 방어 목표 건물(성) 오브젝트의 최상위 컨트롤러입니다.
    /// 기존 CastleView 를 대체합니다.
    /// 로직(Health)과 행동을 분리하여 이벤트 기반으로 감지하도록 설계합니다.
    /// </summary>
    [RequireComponent(typeof(HealthComponent))]
    public class CastleEntity : MonoBehaviour
    {
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.Initialize(500, isEnemy: false); // 아군 기본 세팅
        }

        public void TakeCastleDamage(int amount, Vector3 hitPosition)
        {
            _healthComponent.TakeDamage(amount, hitPosition);
        }
    }
}
