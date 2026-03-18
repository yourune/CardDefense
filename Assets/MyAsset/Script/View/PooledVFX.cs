using UnityEngine;

/// <summary>
/// Auto-return VFX to pool after particle system finishes or after duration
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class PooledVFX : MonoBehaviour
{
    private ParticleSystem particles;
    private System.Action<PooledVFX> returnCallback;
    private float maxDuration = 5f; // Fallback duration
    private float spawnTime;
    
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    
    private void OnEnable()
    {
        spawnTime = Time.time;
        if (particles != null)
        {
            particles.Play();
        }
    }
    
    private void Update()
    {
        // Auto-return when particle system finished or timeout
        if (particles != null && !particles.IsAlive())
        {
            ReturnToPool();
        }
        else if (Time.time - spawnTime > maxDuration)
        {
            ReturnToPool();
        }
    }
    
    public void SetReturnCallback(System.Action<PooledVFX> callback)
    {
        returnCallback = callback;
    }
    
    public void ReturnToPool()
    {
        returnCallback?.Invoke(this);
    }
    
    public void ResetVFX()
    {
        spawnTime = Time.time;
        if (particles != null)
        {
            particles.Clear();
            particles.Play();
        }
    }
}
