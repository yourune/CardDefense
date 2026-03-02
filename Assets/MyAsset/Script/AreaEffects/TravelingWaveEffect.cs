using System.Collections;
using UnityEngine;

[System.Serializable]
public class TravelingWaveEffect : AreaEffect
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float waveSpeed = 10f;
    [SerializeField] private float waveWidth = 5f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private GameObject waveVFXPrefab;
    [SerializeField] private LayerMask enemyLayer;
    
    public override GameAction[] GetGameActions(Vector3[] positions)
    {
        GameAction[] actions = new GameAction[positions.Length];
        
        for (int i = 0; i < positions.Length; i++)
        {
            actions[i] = new TravelingWaveGA(
                positions[i],
                Vector3.right, // Direction (castle to enemy spawn)
                damageAmount,
                waveSpeed,
                waveWidth,
                maxDistance,
                waveVFXPrefab,
                enemyLayer
            );
        }
        
        return actions;
    }
}

/// <summary>
/// GameAction for traveling wave effects (shockwave, wall of fire, etc.)
/// </summary>
public class TravelingWaveGA : GameAction
{
    public Vector3 StartPosition { get; private set; }
    public Vector3 Direction { get; private set; }
    public int DamageAmount { get; private set; }
    public float Speed { get; private set; }
    public float Width { get; private set; }
    public float MaxDistance { get; private set; }
    public GameObject VFXPrefab { get; private set; }
    public LayerMask EnemyLayer { get; private set; }
    
    public TravelingWaveGA(Vector3 startPos, Vector3 dir, int damage, float speed, float width, float maxDist, GameObject vfx, LayerMask enemyLayer)
    {
        this.StartPosition = startPos;
        this.Direction = dir.normalized;
        this.DamageAmount = damage;
        this.Speed = speed;
        this.Width = width;
        this.MaxDistance = maxDist;
        this.VFXPrefab = vfx;
        this.EnemyLayer = enemyLayer;
    }
}
