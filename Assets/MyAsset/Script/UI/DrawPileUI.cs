using TMPro;
using UnityEngine;

public class DrawPileUI : MonoBehaviour
{
    [SerializeField] private TMP_Text drawPileText;

    public void UpdateDrawPileCount(int count)
    {
        drawPileText.text = count.ToString();
    }
}
