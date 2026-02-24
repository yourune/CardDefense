using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    public class HealthAnimations:MonoBehaviour
    {
        [Header("Attacked")]
        [SerializeField] private float _attackedAnimationsDelay = 1f / 9f + 1f / 3f;
        [Header("HPChange")]
        [SerializeField] private HPChangePopup _hpPopupPrefab;

        private ITroopLogic _cardLogic;
        private ICardHPInfoHolder _infoHolder;

        public void Init(ITroopLogic cardLogic, ICardHPInfoHolder infoHolder)
        {
            _cardLogic = cardLogic;
            _infoHolder = infoHolder;

            _cardLogic.OnAttacked += AnimateAttacked;
            _cardLogic.OnHeal += AnimateHeal;
            _cardLogic.OnRemoveHP += ShowRemoveHP;

            if (_cardLogic is IUnitLogic unitLogic)
            {
                unitLogic.OnCounterAttacked += AnimateAttacked;
            }
        }

        private void ShowRemoveHP(int removed)
        {
            _infoHolder.ChangeHP(_cardLogic.HP);
        }

        private void AnimateHeal(int heal)
        {
            Instantiate(_hpPopupPrefab, transform.position, Quaternion.identity).Show(heal);
            _infoHolder.ChangeHP(_cardLogic.HP);
        }

        private void AnimateDamage(int damage)
        {
            Instantiate(_hpPopupPrefab, transform.position, Quaternion.identity).Show(-damage);
        }

        public async void AnimateAttacked(int damage)
        {
            await UniTask.Delay((int)(_attackedAnimationsDelay * 1000));

            _infoHolder.ChangeHP(_cardLogic.HP);
            AnimateDamage(damage);
        }
    }
}
