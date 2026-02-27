using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingRewardText : MonoBehaviour
{
    [SerializeField] private TextMeshPro goldText;
    [SerializeField] private TextMeshPro xpText;
    [SerializeField] private float floatDuration = 1.5f;
    [SerializeField] private float floatHeight = 1.5f;
    
    [Header("Colors")]
    [SerializeField] private Color goldColor = new Color(1f, 0.84f, 0f); // Gold color
    [SerializeField] private Color xpColor = new Color(0.53f, 0.81f, 1f); // Light blue
    
    public void Setup(int gold, int xp)
    {
        if (goldText != null && gold > 0)
        {
            goldText.text = $"+{gold}G";
            goldText.color = goldColor;
            goldText.gameObject.SetActive(true);
        }
        else if (goldText != null)
        {
            goldText.gameObject.SetActive(false);
        }
        
        if (xpText != null && xp > 0)
        {
            xpText.text = $"+{xp}XP";
            xpText.color = xpColor;
            xpText.gameObject.SetActive(true);
        }
        else if (xpText != null)
        {
            xpText.gameObject.SetActive(false);
        }
        
        AnimateAndDestroy();
    }
    
    private void AnimateAndDestroy()
    {
        Sequence sequence = DOTween.Sequence();
        
        // Float up
        sequence.Append(transform.DOMoveY(transform.position.y + floatHeight, floatDuration).SetEase(Ease.OutQuad));
        
        // Fade out using DOColor to change alpha
        if (goldText != null && goldText.gameObject.activeSelf)
        {
            Color fadeGold = goldColor;
            fadeGold.a = 0;
            sequence.Join(goldText.DOColor(fadeGold, floatDuration * 0.7f).SetDelay(floatDuration * 0.3f));
        }
        if (xpText != null && xpText.gameObject.activeSelf)
        {
            Color fadeXp = xpColor;
            fadeXp.a = 0;
            sequence.Join(xpText.DOColor(fadeXp, floatDuration * 0.7f).SetDelay(floatDuration * 0.3f));
        }
        
        // Destroy
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
