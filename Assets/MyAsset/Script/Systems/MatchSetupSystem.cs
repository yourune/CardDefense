using UnityEngine;
using System.Collections.Generic;
public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private CastleData CastleData;
    private void Start()
    {
        CastleSystem.Instance.Setup(CastleData);
        CardSystem.Instance.SetUp(CastleData.Deck);
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
