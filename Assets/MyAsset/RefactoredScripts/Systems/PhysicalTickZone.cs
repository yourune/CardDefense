using UnityEngine;
using CardDefense.Core.Events;
using CardDefense.Entities;

namespace CardDefense.Systems.Combat
{
    /// <summary>
    /// 지속형 장판(독 장판, 회복 장판 등)의 껍데기입니다. 
    /// 기존 EffectZone 을 대체하며 물리 트리거(Tick) 기반으로 작동합니다.
    /// 카드를 내면 이 오브젝트가 생성되어 독립적으로 작동합니다. 
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class PhysicalTickZone : MonoBehaviour
    {
        [Header("Zone Settings")]
        [SerializeField] private float _radius = 3f;
        [SerializeField] private int _damagePerTick = 10;
        [SerializeField] private float _tickInterval = 1f; // 1초마다 데미지
        [SerializeField] private float _lifeTime = 5f;     // 유지 시간

        [SerializeField] private LayerMask _enemyLayer;

        private SphereCollider _collider;
        private float _tickTimer = 0f;
        private float _lifeTimer = 0f;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true; 
        }

        private void OnEnable()
        {
            _tickTimer = 0f;
            _lifeTimer = 0f;
            _collider.radius = _radius;
        }

        private void Update()
        {
            // 장판 수명 체크
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= _lifeTime)
            {
                // TODO: 오브젝트 풀을 사용한다면 Return 처리
                Destroy(gameObject);
                return;
            }

            // 틱(Tick) 마다 물리 범위 안에 있는 적들에게 데미지
            _tickTimer += Time.deltaTime;
            if (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                ApplyTickEffect();
            }
        }

        private void ApplyTickEffect()
        {
            // 이전에 만든 AoECombatHelper를 사용하여 이 장판 반경 안에 있는 적 피격
            AoECombatHelper.DealDamageInArea(transform.position, _radius, _damagePerTick, _enemyLayer);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, _radius);
        }
#endif
    }
}
