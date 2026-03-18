using UnityEngine;
using CardDefense.Systems.Combat;
using CardDefense.Core.Events;

namespace CardDefense.Core.Effects
{
    [System.Serializable]
    public class InstantDamageEffect : IEngineEffect
    {
        public int Damage = 50;
        public float Radius = 3f;
        public LayerMask EnemyLayer;

        public void Execute(Vector3 targetPosition, GameObject targetEntity = null)
        {
            // Fireball, Thunderbolt 등 즉발 광역 또는 단일 타격
            if (Radius > 0)
            {
                AoECombatHelper.DealDamageInArea(targetPosition, Radius, Damage, EnemyLayer);
            }
            else if (targetEntity != null && targetEntity.TryGetComponent(out Entities.HealthComponent health))
            {
                // 단일 타겟팅
                health.TakeDamage(Damage, targetEntity.transform.position);
            }
        }
    }
    
    [System.Serializable]
    public class SpawnZoneEffect : IEngineEffect
    {
        public GameObject ZonePrefab;

        public void Execute(Vector3 targetPosition, GameObject targetEntity = null)
        {
            if (ZonePrefab != null)
            {
                // TODO: Object Pooling 적용
                Object.Instantiate(ZonePrefab, targetPosition, Quaternion.identity);
            }
        }
    }

    [System.Serializable]
    public class DrawCardEffect : IEngineEffect
    {
        public int Amount = 1;

        public void Execute(Vector3 targetPosition, GameObject targetEntity = null)
        {
            // EventBus를 통해 드로우 요청을 보냄 (HandSystem이 수신)
            EventBus.Publish(new TriggerDrawCardEvent { Amount = Amount });
        }
    }
}