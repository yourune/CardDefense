using System;
using System.Collections;
using UnityEngine;

public class RewardSystem : Singleton<RewardSystem>
{
    [Header("Resources")]
    [SerializeField] private int startingGold = 0;
    [SerializeField] private int startingLevel = 1;
    
    [Header("Level Progression")]
    [SerializeField] private int baseXpRequired = 100;
    [SerializeField] private float xpScalingFactor = 1.5f;
    [SerializeField] private int healthPerLevel = 10;
    
    public int CurrentGold { get; private set; }
    public int CurrentXp { get; private set; }
    public int CurrentLevel { get; private set; }
    public int XpRequiredForNextLevel { get; private set; }
    
    public event Action<int> OnGoldChanged;
    public event Action<int, int, int> OnXpChanged; // currentXp, required, level
    public event Action<int> OnLevelUp;
    
    void OnEnable()
    {
        ActionSystem.AttachPerformer<GainGoldGA>(GainGoldPerformer);
        ActionSystem.AttachPerformer<GainXpGA>(GainXpPerformer);
        ActionSystem.AttachPerformer<LevelUpGA>(LevelUpPerformer);
    }
    
    void OnDisable()
    {
        ActionSystem.DetachPerformer<GainGoldGA>();
        ActionSystem.DetachPerformer<GainXpGA>();
        ActionSystem.DetachPerformer<LevelUpGA>();
    }
    
    private void Start()
    {
        CurrentGold = startingGold;
        CurrentLevel = startingLevel;
        CurrentXp = 0;
        CalculateXpRequired();
        
        // Notify UI of initial values
        OnGoldChanged?.Invoke(CurrentGold);
        OnXpChanged?.Invoke(CurrentXp, XpRequiredForNextLevel, CurrentLevel);
    }
    
    private void CalculateXpRequired()
    {
        XpRequiredForNextLevel = Mathf.RoundToInt(baseXpRequired * Mathf.Pow(xpScalingFactor, CurrentLevel - 1));
    }
    
    private IEnumerator GainGoldPerformer(GainGoldGA gainGoldGA)
    {
        CurrentGold += gainGoldGA.Amount;
        OnGoldChanged?.Invoke(CurrentGold);
        yield return null;
    }
    
    private IEnumerator GainXpPerformer(GainXpGA gainXpGA)
    {
        CurrentXp += gainXpGA.Amount;
        
        // Check for level up
        if (CurrentXp >= XpRequiredForNextLevel)
        {
            CurrentXp -= XpRequiredForNextLevel;
            LevelUpGA levelUpGA = new LevelUpGA(CurrentLevel + 1);
            ActionSystem.Instance.AddReaction(levelUpGA);
        }
        
        OnXpChanged?.Invoke(CurrentXp, XpRequiredForNextLevel, CurrentLevel);
        yield return null;
    }
    
    private IEnumerator LevelUpPerformer(LevelUpGA levelUpGA)
    {
        CurrentLevel = levelUpGA.NewLevel;
        CalculateXpRequired();
        
        // Increase castle health
        if (CastleSystem.Instance != null && CastleSystem.Instance.CastleView != null)
        {
            CastleSystem.Instance.CastleView.IncreaseMaxHealth(healthPerLevel);
        }
        
        OnLevelUp?.Invoke(CurrentLevel);
        OnXpChanged?.Invoke(CurrentXp, XpRequiredForNextLevel, CurrentLevel);
        
        Debug.Log($"Level Up! New Level: {CurrentLevel}, Next Level Requires: {XpRequiredForNextLevel} XP");
        yield return null;
    }
    
    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            OnGoldChanged?.Invoke(CurrentGold);
            return true;
        }
        return false;
    }
}
