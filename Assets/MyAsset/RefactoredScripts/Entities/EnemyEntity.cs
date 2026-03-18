using System;
using UnityEngine;
using CardDefense.Core.Events;

namespace CardDefense.Entities
{
    /// <summary>
    /// 적군(Enemy) 오브젝트의 최상위 컨트롤러입니다.
    /// 기존 EnemyView 를 대체합니다.
    /// 로직(Health)과 행동(AI)을 분리하여 이벤트 기반으로 감지하도록 설계합니다.
    /// </summary>
    [RequireComponent(typeof(HealthComponent))]
    public class EnemyEntity : MonoBehaviour
    {
        private HealthComponent _healthComponent;
        private EnemyData _enemyData;

        public HealthComponent Health => _healthComponent;
        public EnemyData Data => _enemyData;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
        }

        /// <summary>
        /// 오브젝트 풀이나 스포너에서 적을 생성할 때 호출합니다.
        /// 기존의 ScriptableObject 데이터를 주입하여 세팅합니다.
        /// </summary>
        public void Setup(EnemyData data)
        {
            _enemyData = data;
            
            // ScriptableObject에 정의된 체력으로 초기화
            int initialHealth = data != null ? data.Health : 100;
            _healthComponent.Initialize(initialHealth, isEnemy: true);
            
            // TODO: 이동 속도나 기타 AI 세팅 추가 가능
        }

        public void ApplyDamage(int amount, Vector3 hitPosition)
        {
            _healthComponent.TakeDamage(amount, hitPosition);
        }
    }
}
