using SerializeReferenceEditor;
using UnityEngine;

[System.Serializable]
public class AutoAreaEffect
{
    [field: SerializeReference, SR] public PositionTargetMode PositionTargetMode { get; private set; }
    [field: SerializeReference, SR] public AreaEffect AreaEffect { get; private set; }
}
