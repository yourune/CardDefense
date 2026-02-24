using UnityEngine;

namespace BG_Games.Card_Game_Core.Visual
{
    public abstract class AimVisual:MonoBehaviour
    {

        public abstract void ShowAim();
        public abstract void HideAim();
        public abstract void SetStartPosition(Vector3 position);
        public abstract void AnimateAim(Vector3 pointerScreenPosition);
    }
}
