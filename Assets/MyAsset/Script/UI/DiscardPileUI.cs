using TMPro;
using UnityEngine;

public class DiscardPileUI : MonoBehaviour
{
    [SerializeField] private TMP_Text discardPileText;

    public void UpdateDiscardPileCount(int count)
    {
        discardPileText.text = count.ToString();
    }
}
