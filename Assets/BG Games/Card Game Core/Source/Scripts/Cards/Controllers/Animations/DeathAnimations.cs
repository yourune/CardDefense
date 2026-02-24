using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    public class DeathAnimations:MonoBehaviour
    {
        [SerializeField] private float _animationDelay = 0.64444444444f;
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _finalScale = 0.2f;

        private ITroopLogic _cardLogic;

        public float AnimationsDuration => _animationDelay + _animationDuration;

        public void Init(ITroopLogic cardLogic)
        {
            _cardLogic = cardLogic;

            _cardLogic.OnDead += AnimateDead;
        }

        public async void AnimateDead()
        {
            await UniTask.Delay((int)(_animationDelay * 1000));
            
            if (this == null) return;
            
            StartCoroutine(TweenAnimation.ScaleTo(
                transform, 
                Vector3.one * _finalScale, 
                _animationDuration, 
                TweenAnimation.EaseInBack, 
                () =>
                {
                    if (this != null)
                    {
                        Destroy(gameObject);
                    }
                }
            ));
        }
    }
}
