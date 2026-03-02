using UnityEngine;

[System.Serializable]
public class SpawnZoneEffect : AreaEffect
{
    [SerializeField] private GameObject zonePrefab;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float radius = 3f;
    
    public override GameAction[] GetGameActions(Vector3[] positions)
    {
        GameAction[] actions = new GameAction[positions.Length];
        
        for (int i = 0; i < positions.Length; i++)
        {
            actions[i] = new SpawnEffectZoneGA(zonePrefab, positions[i], duration, radius);
        }
        
        return actions;
    }
}
