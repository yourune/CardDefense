using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Splines;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using System.Linq;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    
    private readonly List<CardView> cardss = new();

    public IEnumerator AddCard(CardView cardView)
    {
        cardss.Add(cardView);
        yield return UpdateCardPositions(0.15f);
    }

    public CardView RemoveCard(Cards cards)
    {
        CardView cardView = GetCardView(cards);
        if (cardView == null) return null;  
        cardss.Remove(cardView);
        StartCoroutine(UpdateCardPositions(0.15f));
        return cardView;
    }

    private CardView GetCardView(Cards cards)
    {
        return cardss.Where(cardview => cardview.Cards == cards).FirstOrDefault();
    }
    private IEnumerator UpdateCardPositions(float duration)
    {
        if (cardss.Count == 0) yield break;

        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (cardss.Count - 1) * cardSpacing / 2;

        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cardss.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cardss[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cardss[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }
}