using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Cards.Controllers;
using DG.Tweening;
public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;

    [SerializeField] private Transform drawPilePoint;

    [SerializeField] private Transform discardPilePoint;

    //private readonly list of drawpile, discardpile, and hand
    private readonly List<Cards> drawPile = new();
    private readonly List<Cards> discardPile = new();
    private readonly List<Cards> hand = new();

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if (notDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }
    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (var card in hand)
        {
            discardPile.Add(card);
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Cards);
        CardView cardView = handView.RemoveCard(playCardGA.Cards);
        yield return DiscardCard(cardView);

        SpendManaGA spendManaGA = new(playCardGA.Cards.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        //Perform effects
        foreach (var effect in playCardGA.Cards.Effects)
        {
            PerformEffectGA performEffectGa = new(effect);
            ActionSystem.Instance.AddReaction(performEffectGa);
        }
    }

    //publics

    public void SetUp(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Cards cards = new (cardData);
            drawPile.Add(cards);
        }
    }

    //helpers
    private IEnumerator DrawCard()
    {
        Cards cards = drawPile.Draw();
        hand.Add(cards);
        CardView cardView = CardViewCreator.Instance.CreateCardView(cards, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
