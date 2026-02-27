using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Castle")]
public class CastleData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField] public List<CardData> Deck { get; private set; }
 }
