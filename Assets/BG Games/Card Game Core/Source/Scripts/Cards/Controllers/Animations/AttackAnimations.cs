using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using BG_Games.Card_Game_Core;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    public class AttackAnimations:MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private float _attackAnimationsSpeed = 3f;
        [SerializeField] private float _attackAnimationsRatio = 3f / 1f;
        [SerializeField] private float _targetOffset = 0.5f;
        [SerializeField] private float _attackMaxDuration = 0.3f;

        private IUnitLogic _cardLogic;

        public void Init(IUnitLogic cardLogic)
        {
            _cardLogic = cardLogic;

            _cardLogic.OnAttack += AnimateAttack;
        }

        public void AnimateAttack(Vector3 target)
        {
            Vector3 startPos = transform.position;
            Vector3 direction = (target - startPos).normalized;
            float distance = Vector3.Distance(startPos, target) - _targetOffset;
            Vector3 adjustedTarget = startPos + direction * distance;

            float approachSpeed = _attackAnimationsSpeed * _attackAnimationsRatio;
            float returnSpeed = _attackAnimationsSpeed;

            float approachDuration = Mathf.Clamp(distance / approachSpeed, 0, _attackMaxDuration);
            float returnDuration = Mathf.Clamp(distance / returnSpeed, 0, _attackMaxDuration * _attackAnimationsRatio);
            
            StartCoroutine(TweenAnimation.MoveTo(
                transform, 
                adjustedTarget, 
                approachDuration, 
                TweenAnimation.EaseInBack, 
                () => 
                {
                    StartCoroutine(TweenAnimation.MoveTo(
                        transform, 
                        startPos, 
                        returnDuration, 
                        TweenAnimation.EaseOutCubic
                    ));
                }
            ));
        }
    }
}
