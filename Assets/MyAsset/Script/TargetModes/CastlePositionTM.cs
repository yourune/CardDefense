using UnityEngine;

[System.Serializable]
public class CastlePositionTM : PositionTargetMode
{
    /// <summary>
    /// Returns the castle position (for shockwave effects)
    /// </summary>
    public override Vector3[] GetPositions()
    {
        if (CastleSystem.Instance != null && CastleSystem.Instance.CastleView != null)
        {
            return new Vector3[] { CastleSystem.Instance.CastleView.transform.position };
        }
        
        Debug.LogError("CastlePositionTM: Castle not found!");
        return new Vector3[] { Vector3.zero };
    }
}
