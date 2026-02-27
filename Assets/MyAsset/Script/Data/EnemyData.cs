using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public float Speed { get; private set; } = 2f;
    
    [Header("Rewards")]
    [field: SerializeField] public int GoldDrop { get; private set; } = 10;
    [field: SerializeField] public int XpDrop { get; private set; } = 5;
}
