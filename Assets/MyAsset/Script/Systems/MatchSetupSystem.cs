using UnityEngine;
using System.Collections.Generic;
public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private CastleData CastleData;
    [SerializeField] private List<EnemyData> EnemyDataList;
    private void Start()
    {
        CastleSystem.Instance.Setup(CastleData);
        EnemySystem.Instance.SpawnEnemy(EnemyDataList);
        CardSystem.Instance.SetUp(CastleData.Deck);
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
