using UnityEngine;

namespace BG_Games.Card_Game_Core.Tools
{
    public class AspectRatioFilter : MonoBehaviour
    {
        [SerializeField] private float MaxRatio;

        private Camera _mainCamera;

        void Start()
        {
            _mainCamera = Camera.main;

            if (_mainCamera.aspect > MaxRatio)
            {
                float currentAspect = (float)Screen.width / Screen.height;
                float fov = Mathf.Rad2Deg * Mathf.Atan(Mathf.Tan(_mainCamera.fieldOfView * Mathf.Deg2Rad / 2f) * (MaxRatio / currentAspect) * 2f);
                _mainCamera.fieldOfView = fov;

                float panelScale = (currentAspect / MaxRatio);
                _mainCamera.rect = new Rect((1 - panelScale) / 2, 0, 1, 1);

            }
        }
    }
}
