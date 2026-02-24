using UnityEngine;

public class CastleDamageTestSystem : MonoBehaviour
{
    [SerializeField] private CombatantView castle;
    [SerializeField] private int damagePerSecond = 5;

    private void Start()
    {
        if (castle != null)
        {
            InvokeRepeating(nameof(DamageCastle), 1f, 1f);
        }
        else
        {
            Debug.LogError("Castle reference is not assigned!");
        }
    }

    private void DamageCastle()
    {
        castle.TakeDamage(damagePerSecond);
        Debug.Log($"Castle took {damagePerSecond} damage! Current HP: {castle.currentHealth}");
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(DamageCastle));
    }
}
