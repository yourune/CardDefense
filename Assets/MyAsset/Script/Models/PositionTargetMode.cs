using UnityEngine;

[System.Serializable]
public abstract class PositionTargetMode
{
    /// <summary>
    /// Returns the world position(s) where the effect should be applied
    /// </summary>
    public abstract Vector3[] GetPositions();
}
