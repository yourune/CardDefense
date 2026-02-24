using UnityEngine;

namespace BG_Games.Card_Game_Core.UI
{
    public class CanvasTopLayerHauler : MonoBehaviour, ITopLayerHauler
    {
        [SerializeField] private RectTransform _screenOfTopCanvas;

        public void BringToTopLayer(Transform uiElement)
        {
            uiElement.parent = _screenOfTopCanvas;
        }
    }
}