using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Card_Game_Core.Tools
{
    
    class MaxAspectRatioFitter:UIBehaviour
    {
        [SerializeField] private float MaxRatio;

        protected override void Start()
        {
            RectTransform rectTransform = transform as RectTransform;

            float aspect = rectTransform.rect.width / rectTransform.rect.height;

            if (aspect > MaxRatio)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * MaxRatio);
            }
        }
    }
}
