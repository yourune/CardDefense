using System;
using UnityEngine;
using CardDefense.Core.Events;

namespace CardDefense.Entities
{
    /// <summary>
    /// 체력을 가지고 상호작용(데미지, 회복 등)을 처리하는 순수 로직 컴포넌트입니다.
    /// 기존 CombatantView의 로직 부분을 분리했습니다.
    /// 코루틴 대기 없이 즉각적으로 체력을 계산하고 이벤트를 발생시킵니다.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 100;
        
        // 인스펙터에서 깎이는 것을 눈으로 확인하기 위해 직렬화 추가
        [SerializeField] private int _currentHealth;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;

        // 적군인지 아군(성)인지 구분용
        [SerializeField] private bool _isEnemy = true; 

        private void Start()
        {
            // 컴포넌트 활성화 시 체력 초기화
            if (_currentHealth == 0) 
            {
                ResetHealth();
            }
        }

        public void Initialize(int maxHealth, bool isEnemy)
        {
            _maxHealth = maxHealth;
            _isEnemy = isEnemy;
            ResetHealth();
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
        }

        /// <summary>
        /// 1프레임 내에 즉각적으로 데미지를 처리하고 이벤트를 발생시킵니다.
        /// </summary>
        public void TakeDamage(int damageAmount, Vector3 hitPosition)
        {
            if (IsDead || damageAmount <= 0) return;

            _currentHealth = Mathf.Max(0, _currentHealth - damageAmount);

            // 1. 데미지 입음 이벤트 발행 (VFX, 텍스트 시스템이 수신)
            EventBus.Publish(new EntityTakeDamageEvent
            {
                Target = this.gameObject,
                DamageAmount = damageAmount,
                HitPosition = hitPosition
            });

            // 2. 사망 체크 및 사망 이벤트 발행
            if (IsDead)
            {
                Die();
            }
        }

        private void Die()
        {
            // 사망 이벤트 발행 (게임오버 처리, 적 킬 카운트, 풀 반환 등의 시스템이 수신)
            EventBus.Publish(new EntityDeathEvent
            {
                DeadEntity = this.gameObject,
                IsEnemy = _isEnemy
            });

            // 참고: 여기서 바로 gameObject.SetActive(false)를 하지 않습니다.
            // 데스 애니메이션이 끝난 후 풀에 반환되도록 분리하는 것이 좋습니다.
        }
    }
}
