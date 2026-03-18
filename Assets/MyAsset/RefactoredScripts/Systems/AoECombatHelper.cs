using System.Collections.Generic;
using UnityEngine;
using CardDefense.Entities;

namespace CardDefense.Systems.Combat
{
    /// <summary>
    /// 장판(Zone) 기반의 범위 공격/효과를 처리하는 핵심 클래스입니다.
    /// 기존 코루틴 대기열 대신, Unity 물리 엔진(OverlapSphere 등)을 활용해 
    /// 지정된 범위 내의 모든 적을 즉시(프레임 지연 없이) 찾아내어 데미지를 입힙니다.
    /// </summary>
    public class AoECombatHelper : MonoBehaviour
    {
        // 최적화를 위해 Overlap 결과를 담을 미리 할당된 배열 (가비지 컬렉션 방지)
        // 최대 한 번에 hit할 타겟 수 (웨이브가 많으므로 넉넉히 100개)
        private static readonly Collider[] _hitColliders = new Collider[100]; 

        /// <summary>
        /// 특정 위치 반경 내의 적들에게 즉발 데미지를 줍니다.
        /// </summary>
        public static void DealDamageInArea(Vector3 center, float radius, int damageAmount, LayerMask enemyLayer)
        {
            // Physics.OverlapSphereNonAlloc : 기존 OverlapSphere의 메모리 쓰레기 발생 문제 해결 버전
            int hitCount = Physics.OverlapSphereNonAlloc(center, radius, _hitColliders, enemyLayer);

            for (int i = 0; i < hitCount; i++)
            {
                Collider col = _hitColliders[i];
                
                // 해당 범위 내에 들어온 충돌체(주로 적) 중에서 HealthComponent를 찾음
                if (col.TryGetComponent(out HealthComponent targetHealth))
                {
                    // 즉시 타격 (코루틴 대기 없음)
                    targetHealth.TakeDamage(damageAmount, col.transform.position);
                }
            }
        }
    }
}
