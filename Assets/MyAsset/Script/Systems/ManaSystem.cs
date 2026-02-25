using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;
    [SerializeField] private float manaRefillInterval = 3f;
    private const int MAX_MANA = 10;
    private const int STARTING_MANA = 5;
    private int currentMana = STARTING_MANA;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        StopAllCoroutines();
    }

    private void Start()
    {
        if (manaUI != null)
        {
            manaUI.UpdateManaText(currentMana);
        }
        StartCoroutine(AutoRefillMana());
    }

    private IEnumerator AutoRefillMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaRefillInterval);
            
            if (currentMana < MAX_MANA)
            {
                RefillManaGA refillManaGA = new();
                ActionSystem.Instance.Perform(refillManaGA);
            }
        }
    }

    public bool HasEnoughMana(int amount)
    {
        return currentMana >= amount;
    }
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        currentMana -= spendManaGA.Amount;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        //refill mana+1 up to max mana
        currentMana = Mathf.Min(currentMana + 1, MAX_MANA);
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }
}
