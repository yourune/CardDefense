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
    
    [Header("Spawn Settings")]
    [SerializeField] private int goldPerCoin = 20; // Spawn 1 coin per 20 gold
    [SerializeField] private int xpPerOrb = 10; // Spawn 1 orb per 10 XP
    [SerializeField] private int maxCoinsPerDrop = 3; // Maximum visual coins
    [SerializeField] private int maxOrbsPerDrop = 3; // Maximum visual orbs
    
    [Header("Object Pool Settings")]
    [SerializeField] private int initialPoolSize = 20; // Pre-instantiate objects
    
    [Header("Accumulation Settings")]
    [SerializeField] private float displayInterval = 5f; // Show accumulated rewards every 5 seconds
    
    private Camera mainCamera;
    private int accumulatedGold = 0;
    private int accumulatedXp = 0;
    
    // Object pools
    private SimpleObjectPool<GoldCoin> goldCoinPool;
    private SimpleObjectPool<XpOrb> xpOrbPool;
    private Transform poolParent;
    
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DropRewardGA>(DropRewardPerformer);
        ActionSystem.AttachPerformer<DropMultipleRewardsGA>(DropMultipleRewardsPerformer);
    }
    
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DropRewardGA>();
        ActionSystem.DetachPerformer<DropMultipleRewardsGA>();
        StopAllCoroutines();
        
        // Return all objects to pool
        goldCoinPool?.ReturnAll();
        xpOrbPool?.ReturnAll();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        // Create pool parent
        poolParent = new GameObject("RewardObjectPools").transform;
        poolParent.SetParent(transform);
        
        // Initialize object pools
        if (goldCoinPrefab != null)
        {
            goldCoinPool = new SimpleObjectPool<GoldCoin>(goldCoinPrefab, poolParent, initialPoolSize);
        }
        
        if (xpOrbPrefab != null)
        {
            xpOrbPool = new SimpleObjectPool<XpOrb>(xpOrbPrefab, poolParent, initialPoolSize);
        }
        
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
        if (goldAmount > 0 && goldCoinPool != null)
        {
            // Reduced spawn count: 1 coin per 20 gold, minimum 1, maximum 3
            int coinsToSpawn = Mathf.Clamp((goldAmount / goldPerCoin) + 1, 1, maxCoinsPerDrop);
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnAndAnimateReward(goldCoinPool, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Drop XP orbs to castle
        if (xpAmount > 0 && xpOrbPool != null)
        {
            // Reduced spawn count: 1 orb per 10 XP, minimum 1, maximum 3
            int orbsToSpawn = Mathf.Clamp((xpAmount / xpPerOrb) + 1, 1, maxOrbsPerDrop);
            for (int i = 0; i < orbsToSpawn; i++)
            {
                SpawnAndAnimateReward(xpOrbPool, dropPosition, castlePosition);
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
        if (dropRewardGA.GoldAmount > 0 && goldCoinPool != null)
        {
            // Reduced spawn count: 1 coin per 20 gold, minimum 1, maximum 3
            int coinsToSpawn = Mathf.Clamp((dropRewardGA.GoldAmount / goldPerCoin) + 1, 1, maxCoinsPerDrop);
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnAndAnimateReward(goldCoinPool, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Drop XP orbs to castle
        if (dropRewardGA.XpAmount > 0 && xpOrbPool != null)
        {
            // Reduced spawn count: 1 orb per 10 XP, minimum 1, maximum 3
            int orbsToSpawn = Mathf.Clamp((dropRewardGA.XpAmount / xpPerOrb) + 1, 1, maxOrbsPerDrop);
            for (int i = 0; i < orbsToSpawn; i++)
            {
                SpawnAndAnimateReward(xpOrbPool, dropPosition, castlePosition);
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
    
    /// <summary>
    /// Drop multiple rewards simultaneously (all spawn at once)
    /// </summary>
    private IEnumerator DropMultipleRewardsPerformer(DropMultipleRewardsGA dropMultipleRewardsGA)
    {
        if (dropMultipleRewardsGA.Rewards == null || dropMultipleRewardsGA.Rewards.Count == 0)
        {
            yield break;
        }
        
        Vector3 castlePosition = GetCastlePosition();
        int totalGold = 0;
        int totalXp = 0;
        
        // Spawn all rewards simultaneously
        foreach (var (position, gold, xp) in dropMultipleRewardsGA.Rewards)
        {
            totalGold += gold;
            totalXp += xp;
            
            // Start spawning coins/orbs for this reward (don't wait)
            StartCoroutine(SpawnRewardVisuals(position, castlePosition, gold, xp));
        }
        
        // Wait for all animations to complete
        yield return new WaitForSeconds(collectDuration * 1.5f + dropDelay * maxCoinsPerDrop);
        
        // Accumulate all rewards
        accumulatedGold += totalGold;
        accumulatedXp += totalXp;
        
        // Gain resources
        if (totalGold > 0)
        {
            GainGoldGA gainGoldGA = new GainGoldGA(totalGold);
            ActionSystem.Instance.AddReaction(gainGoldGA);
        }
        
        if (totalXp > 0)
        {
            GainXpGA gainXpGA = new GainXpGA(totalXp);
            ActionSystem.Instance.AddReaction(gainXpGA);
        }
    }
    
    /// <summary>
    /// Helper method to spawn reward visuals for a single location
    /// </summary>
    private IEnumerator SpawnRewardVisuals(Vector3 dropPosition, Vector3 castlePosition, int goldAmount, int xpAmount)
    {
        // Drop gold coins
        if (goldAmount > 0 && goldCoinPool != null)
        {
            int coinsToSpawn = Mathf.Clamp((goldAmount / goldPerCoin) + 1, 1, maxCoinsPerDrop);
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnAndAnimateReward(goldCoinPool, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
        
        // Drop XP orbs
        if (xpAmount > 0 && xpOrbPool != null)
        {
            int orbsToSpawn = Mathf.Clamp((xpAmount / xpPerOrb) + 1, 1, maxOrbsPerDrop);
            for (int i = 0; i < orbsToSpawn; i++)
            {
                SpawnAndAnimateReward(xpOrbPool, dropPosition, castlePosition);
                yield return new WaitForSeconds(dropDelay);
            }
        }
    }
    
    private void SpawnAndAnimateReward<T>(SimpleObjectPool<T> pool, Vector3 startPos, Vector3 targetPos) where T : Component
    {
        T reward = pool.Get();
        reward.transform.position = startPos;
        reward.transform.localScale = Vector3.one;
        
        // Setup return callback
        if (reward is GoldCoin goldCoin)
        {
            goldCoin.ResetCoin();
            goldCoin.SetReturnCallback((coin) => goldCoinPool.Return(coin));
        }
        else if (reward is XpOrb xpOrb)
        {
            xpOrb.ResetOrb();
            xpOrb.SetReturnCallback((orb) => xpOrbPool.Return(orb));
        }
        
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
        
        // Return to pool after animation
        sequence.OnComplete(() => 
        {
            if (reward is GoldCoin gc)
                gc.ReturnToPool();
            else if (reward is XpOrb xo)
                xo.ReturnToPool();
        });
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
