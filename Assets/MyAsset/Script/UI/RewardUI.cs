using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class RewardUI : MonoBehaviour
{
    [Header("Gold Display")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Image goldIcon;
    
    [Header("XP Display")]
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image xpBarFill;
    
    [Header("Level Up Effect")]
    [SerializeField] private GameObject levelUpVFX;
    [SerializeField] private float vfxDuration = 2f;
    
    private bool isSubscribed = false;
    
    private void OnEnable()
    {
        SubscribeToEvents();
    }
    
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    
    private void Start()
    {
        // Ensure subscription (in case RewardSystem wasn't ready in OnEnable)
        SubscribeToEvents();
        
        // Initialize display
        if (RewardSystem.Instance != null)
        {
            UpdateGoldDisplay(RewardSystem.Instance.CurrentGold);
            UpdateXpDisplay(
                RewardSystem.Instance.CurrentXp, 
                RewardSystem.Instance.XpRequiredForNextLevel,
                RewardSystem.Instance.CurrentLevel
            );
        }
    }
    
    private void SubscribeToEvents()
    {
        if (isSubscribed) return;
        
        if (RewardSystem.Instance != null)
        {
            RewardSystem.Instance.OnGoldChanged += UpdateGoldDisplay;
            RewardSystem.Instance.OnXpChanged += UpdateXpDisplay;
            RewardSystem.Instance.OnLevelUp += OnLevelUp;
            isSubscribed = true;
        }
    }
    
    private void UnsubscribeFromEvents()
    {
        if (!isSubscribed) return;
        
        if (RewardSystem.Instance != null)
        {
            RewardSystem.Instance.OnGoldChanged -= UpdateGoldDisplay;
            RewardSystem.Instance.OnXpChanged -= UpdateXpDisplay;
            RewardSystem.Instance.OnLevelUp -= OnLevelUp;
            isSubscribed = false;
        }
    }
    
    private void UpdateGoldDisplay(int amount)
    {
        if (goldText != null)
        {
            goldText.text = amount.ToString();
        }
        
        // Optional: Add a pop/scale animation
        if (goldIcon != null)
        {
            goldIcon.transform.DOScale(Vector3.one * 1.2f, 0.15f).SetEase(Ease.OutQuad)
                .OnComplete(() => goldIcon.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InQuad));
        }
    }
    
    private void UpdateXpDisplay(int currentXp, int requiredXp, int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Level {level}";
        }
        
        if (xpText != null)
        {
            xpText.text = $"{currentXp}/{requiredXp} XP";
        }
        
        if (xpBarFill != null)
        {
            float fillAmount = (float)currentXp / requiredXp;
            DOTween.To(() => xpBarFill.fillAmount, x => xpBarFill.fillAmount = x, fillAmount, 0.5f)
                .SetEase(Ease.OutQuad);
        }
    }
    
    private void OnLevelUp(int newLevel)
    {
        // Play level up VFX
        if (levelUpVFX != null)
        {
            GameObject vfx = Instantiate(levelUpVFX, transform);
            Destroy(vfx, vfxDuration);
        }
        
        // Animate level text
        if (levelText != null)
        {
            levelText.transform.DOScale(Vector3.one * 1.5f, 0.25f).SetEase(Ease.OutQuad)
                .OnComplete(() => levelText.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutElastic));
        }
        
        Debug.Log($"Level Up VFX! New Level: {newLevel}");
    }
}
