using BG_Games.Card_Game_Core;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    class HPChangePopup:MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;
        [SerializeField] private float _startScale = 0.2f;
        [SerializeField] private float _appearanceDuration = 0.2f;
        [SerializeField] private float _hideDuration = 0.3f;
        [SerializeField] private float _showDuration = 1f;

        public void Show(int delta)
        {
            if (this == null) return;
            
            float defaultScale = 1f;
            _value.text = delta > 0 ? $"+{delta}" : $"{delta}";

            transform.localScale = new Vector3(_startScale, _startScale, _startScale);
            
            StartCoroutine(TweenAnimation.ScaleTo(
                transform, 
                Vector3.one * defaultScale, 
                _appearanceDuration, 
                TweenAnimation.EaseOutElastic, 
                Waiting
            ));
        }

        private async void Waiting()
        {
            await UniTask.Delay((int)(_showDuration * 1000));
            if(this == null) return;
            
            Hide();
        }

        private void Hide()
        {
            StartCoroutine(TweenAnimation.ScaleTo(
                transform, 
                Vector3.one * _startScale, 
                _hideDuration, 
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
