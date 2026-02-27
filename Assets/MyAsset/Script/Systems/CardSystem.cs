using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private DrawPileUI drawPileUI;
    [SerializeField] private DiscardPileUI discardPileUI;

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
            CardView cardView = handView.RemoveCard(card);
            if (cardView != null)
            {
                yield return DiscardCard(cardView);
            }
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Cards);
        CardView cardView = handView.RemoveCard(playCardGA.Cards);
        
        // Only discard if cardView is not null
        if (cardView != null)
        {
            yield return DiscardCard(cardView);
        }

        SpendManaGA spendManaGA = new(playCardGA.Cards.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if (playCardGA.Cards.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGa = new(playCardGA.Cards.ManualTargetEffect, new (){ playCardGA.ManualTarget });
            ActionSystem.Instance.AddReaction(performEffectGa);
        }
        //Perform effects
        foreach (var effectWrapper in playCardGA.Cards.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGA performEffectGa = new(effectWrapper.Effect, targets);
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
        UpdatePileUI();
    }

    //helpers
    private IEnumerator DrawCard()
    {
        Cards cards = drawPile.Draw();
        hand.Add(cards);
        UpdatePileUI();
        CardView cardView = CardViewCreator.Instance.CreateCardView(cards, drawPilePoint.position, drawPilePoint.rotation);
        
        yield return handView.AddCard(cardView, (excessCards) =>
        {
            // Discard excess cards from the left
            foreach (var excessCard in excessCards)
            {
                if (excessCard != null && excessCard.Cards != null)
                {
                    hand.Remove(excessCard.Cards);
                    StartCoroutine(DiscardCard(excessCard));
                }
            }
        });
    }

    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        UpdatePileUI();
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        // Safety check: if cardView is null or already destroyed, skip
        if (cardView == null || cardView.gameObject == null)
        {
            yield break;
        }
        
        // Safety check: if Cards is null, skip adding to discard pile
        if (cardView.Cards != null)
        {
            discardPile.Add(cardView.Cards);
            UpdatePileUI();
        }
        
        // Faster animation: 0.15f -> 0.08f -> 0.04f for quicker card execution
        float animDuration = 0.04f;
        cardView.transform.DOScale(Vector3.zero, animDuration);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, animDuration);
        yield return tween.WaitForCompletion();
        
        // Final safety check before destroying
        if (cardView != null && cardView.gameObject != null)
        {
            Destroy(cardView.gameObject);
        }
    }

    private void UpdatePileUI()
    {
        if (drawPileUI != null)
        {
            drawPileUI.UpdateDrawPileCount(drawPile.Count);
        }
        if (discardPileUI != null)
        {
            discardPileUI.UpdateDiscardPileCount(discardPile.Count);
        }
    }
}
