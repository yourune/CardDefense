using UnityEngine;

[System.Serializable]
public abstract class AreaEffect
{
    /// <summary>
    /// Creates the area effect at the given positions
    /// Returns GameAction(s) to be executed
    /// </summary>
    public abstract GameAction[] GetGameActions(Vector3[] positions);
}
