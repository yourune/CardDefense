using UnityEngine;

[System.Serializable]
public class ManualPositionTM : PositionTargetMode
{
    // This will be set by ManualPositionTargetSystem when user clicks
    private Vector3 selectedPosition;
    
    public void SetPosition(Vector3 position)
    {
        selectedPosition = position;
    }
    
    public override Vector3[] GetPositions()
    {
        return new Vector3[] { selectedPosition };
    }
}
