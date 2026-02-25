using UnityEngine;
using System.Collections;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFXPrefab;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        foreach (var target in dealDamageGA.Targets)
        {
            if (target != null)
            {
                target.TakeDamage(dealDamageGA.DamageAmount);
                if (damageVFXPrefab != null)
                {
                    Instantiate(damageVFXPrefab, target.transform.position, Quaternion.identity);
                }
            }
        }
        yield return new WaitForSeconds(0.15f);
    }
}
