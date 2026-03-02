using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RewardVisualSystem : Singleton<RewardVisualSystem>
{
    [Header("Prefabs")]
    [SerializeField] private GameObject goldCoinPrefab;
    [SerializeField] private GameObject xpOrbPrefab;
    [SerializeField] private GameObject floatingTextPrefab;
    
    [Header("Animation Settings")]
    [SerializeField] private float dropDelay = 0.1f;
    [SerializeField] private float dropDuration = 0.8f;
    [SerializeField] private float dropHeight = 2f;
    [SerializeField] private float collectDuration = 0.5f;
    [SerializeField] private Ease collectEase = Ease.InBack;
    
    [Header("Accumulation Settings")]
    [SerializeField] private float displayInterval = 5f; // Show accumulated rewards every 5 seconds
    
    private Camera mainCamera;
    private int accumulatedGold = 0;
    private int accumulatedXp = 0;
    
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DropRewardGA>(DropRewardPerformer);
    }
    
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DropRewardGA>();
        StopAllCoroutines();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(DisplayAccumulatedRewards());
    }
    
    /// <summary>
    /// Spawn rewards directly without using ActionSystem (for zones)
    /// </summary>
    public void SpawnRewardsDirect(Vector3 dropPosition, int goldAmount, int xpAmount)
    {
        StartCoroutine(SpawnRewardsDirectCoroutine(dropPosition, goldAmount, xpAmount));
    }
    
    private IEnumerator SpawnRewardsDirectCoroutine(Vector3 dropPosition, int goldAmount, int xpAmount)
    {
        Vector3 castlePosition = GetCastlePosition();
        
        // Drop gold coins to castle
        if (goldAmount > 0 && goldCoinPrefab != null)
        {
            int coinsToSpawn = Mathf.Min(goldAmount / 10 + 1, 5);
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnAndAnimateReward(goldCoinPrefab, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Drop XP orbs to castle
        if (xpAmount > 0 && xpOrbPrefab != null)
        {
            int orbsToSpawn = Mathf.Min(xpAmount / 5 + 1, 5);
            for (int i = 0; i < orbsToSpawn; i++)
            {
                SpawnAndAnimateReward(xpOrbPrefab, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Wait for collection animation, then directly add rewards
        yield return new WaitForSeconds(collectDuration * 1.5f);
        
        accumulatedGold += goldAmount;
        accumulatedXp += xpAmount;
        
        // Gain resources directly without ActionSystem
        if (goldAmount > 0 && RewardSystem.Instance != null)
        {
            RewardSystem.Instance.AddGoldDirect(goldAmount);
        }
        
        if (xpAmount > 0 && RewardSystem.Instance != null)
        {
            RewardSystem.Instance.AddXpDirect(xpAmount);
        }
    }
    
    private IEnumerator DropRewardPerformer(DropRewardGA dropRewardGA)
    {
        Vector3 dropPosition = dropRewardGA.DropPosition;
        Vector3 castlePosition = GetCastlePosition();
        
        // Drop gold coins to castle
        if (dropRewardGA.GoldAmount > 0 && goldCoinPrefab != null)
        {
            int coinsToSpawn = Mathf.Min(dropRewardGA.GoldAmount / 10 + 1, 5); // Max 5 visual coins
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnAndAnimateReward(goldCoinPrefab, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Drop XP orbs to castle
        if (dropRewardGA.XpAmount > 0 && xpOrbPrefab != null)
        {
            int orbsToSpawn = Mathf.Min(dropRewardGA.XpAmount / 5 + 1, 5); // Max 5 visual orbs
            for (int i = 0; i < orbsToSpawn; i++)
            {
                SpawnAndAnimateReward(xpOrbPrefab, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Wait for collection animation, then accumulate rewards
        yield return new WaitForSeconds(collectDuration * 1.5f);
        
        accumulatedGold += dropRewardGA.GoldAmount;
        accumulatedXp += dropRewardGA.XpAmount;
        
        // Actually gain the resources
        if (dropRewardGA.GoldAmount > 0)
        {
            GainGoldGA gainGoldGA = new GainGoldGA(dropRewardGA.GoldAmount);
            ActionSystem.Instance.AddReaction(gainGoldGA);
        }
        
        if (dropRewardGA.XpAmount > 0)
        {
            GainXpGA gainXpGA = new GainXpGA(dropRewardGA.XpAmount);
            ActionSystem.Instance.AddReaction(gainXpGA);
        }
    }
    
    private void SpawnAndAnimateReward(GameObject prefab, Vector3 startPos, Vector3 targetPos)
    {
        GameObject reward = Instantiate(prefab, startPos, Quaternion.identity);
        
        // Random offset for drop
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f), 
            Random.Range(-0.3f, 0.3f), 
            0
        );
        Vector3 dropPos = startPos + randomOffset + Vector3.up * dropHeight;
        
        Sequence sequence = DOTween.Sequence();
        
        // Drop down with bounce
        sequence.Append(reward.transform.DOMove(dropPos, dropDuration * 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(reward.transform.DOMove(startPos + randomOffset, dropDuration * 0.3f).SetEase(Ease.InBounce));
        
        // Wait a moment
        sequence.AppendInterval(0.1f);
        
        // Collect to castle
        sequence.Append(reward.transform.DOMove(targetPos, collectDuration).SetEase(collectEase));
        sequence.Join(reward.transform.DOScale(0.3f, collectDuration));
        
        // Destroy after animation
        sequence.OnComplete(() => Destroy(reward));
    }
    
    private void ShowFloatingText(Vector3 position, int gold, int xp)
    {
        if (floatingTextPrefab == null) return;
        
        GameObject textObj = Instantiate(floatingTextPrefab, position + Vector3.up * 0.5f, Quaternion.identity);
        
        FloatingRewardText floatingText = textObj.GetComponent<FloatingRewardText>();
        if (floatingText != null)
        {
            floatingText.Setup(gold, xp);
        }
    }
    
    private IEnumerator DisplayAccumulatedRewards()
    {
        while (true)
        {
            yield return new WaitForSeconds(displayInterval);
            
            // Show floating text at castle if there are accumulated rewards
            if (accumulatedGold > 0 || accumulatedXp > 0)
            {
                Vector3 castlePosition = GetCastlePosition();
                ShowFloatingText(castlePosition, accumulatedGold, accumulatedXp);
                
                // Reset accumulators
                accumulatedGold = 0;
                accumulatedXp = 0;
            }
        }
    }
    
    private Vector3 GetCastlePosition()
    {
        if (CastleSystem.Instance != null && CastleSystem.Instance.CastleView != null)
        {
            return CastleSystem.Instance.CastleView.transform.position;
        }
        
        return Vector3.zero;
    }
}
