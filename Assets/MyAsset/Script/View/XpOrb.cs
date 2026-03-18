using UnityEngine;
using System;

// Simple XP orb visual - attach to a sprite GameObject
public class XpOrb : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.2f;
    
    private Vector3 originalScale;
    private Action<XpOrb> returnToPoolCallback;
    private float timeOffset; // For varied pulsing
    
    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalScale = transform.localScale;
    }
    
    // Add pulsing animation
    private void Update()
    {
        float pulse = 1f + Mathf.Sin((Time.time + timeOffset) * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * pulse;
    }
    
    /// <summary>
    /// Setup callback for returning to pool
    /// </summary>
    public void SetReturnCallback(Action<XpOrb> callback)
    {
        returnToPoolCallback = callback;
    }
    
    /// <summary>
    /// Return this orb to the pool
    /// </summary>
    public void ReturnToPool()
    {
        returnToPoolCallback?.Invoke(this);
    }
    
    /// <summary>
    /// Reset orb state when returned to pool
    /// </summary>
    public void ResetOrb()
    {
        transform.localScale = originalScale;
        timeOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f); // Random start for pulse variety
    }
}
