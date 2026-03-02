using UnityEngine;

[System.Serializable]
public class RandomPositionTM : PositionTargetMode
{
    [SerializeField] private int numberOfPositions = 3;
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minZ = -5f;
    [SerializeField] private float maxZ = 5f;
    [SerializeField] private float yPosition = 0f;
    
    /// <summary>
    /// Returns random positions within specified bounds
    /// </summary>
    public override Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[numberOfPositions];
        
        for (int i = 0; i < numberOfPositions; i++)
        {
            positions[i] = new Vector3(
                Random.Range(minX, maxX),
                yPosition,
                Random.Range(minZ, maxZ)
            );
        }
        
        return positions;
    }
}
