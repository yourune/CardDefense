using UnityEngine;

namespace BG_Games.Card_Game_Core.Visual
{
    class SimpleSight:AimVisual
    {
        [SerializeField] private Transform _sight;

        public override void ShowAim()
        {
            _sight.gameObject.SetActive(true);
        }

        public override void HideAim()
        {
            _sight.gameObject.SetActive(false);
        }

        public override void SetStartPosition(Vector3 position)
        {
            
        }

        public override void AnimateAim(Vector3 pointerScreenPosition)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerScreenPosition);

            worldPosition.z = _sight.position.z;
            _sight.transform.position = worldPosition;
        }
    }
}
