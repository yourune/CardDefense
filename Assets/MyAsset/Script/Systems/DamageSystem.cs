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
                    yield return new WaitForSeconds(0.15f);
                    
                    // Check if target is still valid after wait
                    if(target != null && target.currentHealth <= 0)
                    {
                        if (target is EnemyView enemyView)
                        {
                            // Drop rewards before killing enemy
                            if (enemyView.EnemyData != null)
                            {
                                DropRewardGA dropRewardGA = new DropRewardGA(
                                    enemyView.transform.position,
                                    enemyView.EnemyData.GoldDrop,
                                    enemyView.EnemyData.XpDrop
                                );
                                ActionSystem.Instance.AddReaction(dropRewardGA);
                            }
                            
                            KillEnemyGA killEnemyGA = new(enemyView);
                            ActionSystem.Instance.AddReaction(killEnemyGA);
                        }
                        else
                        {
                            //Do some game over logic here
                            //Open Game Over Scene
                        }
                    }
                }
            }
        
        }
    }   
}
