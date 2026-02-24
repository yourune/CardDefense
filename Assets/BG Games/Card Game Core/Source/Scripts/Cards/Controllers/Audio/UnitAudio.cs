using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Audio
{
    class UnitAudio : CardAudio
    {
        [Space]
        [SerializeField] protected AudioClip[] _actionClips;

        protected AudioClip _effectSound;

        protected Card _card;

        public override void Init(Card card)
        {
            base.Init(card);

            _card = card;

            if (card is UnitCard unitCard)
            {
                unitCard.UnitLogic.OnAttack += AttackSound;

                if (unitCard.UnitInfo.LogicFactory is ISFXInfo effectFactory)
                {
                    _effectSound = effectFactory.SFX;
                }

                if (unitCard.Logic is ILogicWithSFX effect)
                {
                    effect.OnSFXApply += AbillitySound;
                }
            }

        }

        private void AbillitySound()
        {
            if (_effectSound != null)
            {
                AudioSystem.PlaySFX(_effectSound);
            }
        }

        private void AttackSound(Vector3 target)
        {
            int clipIndex = Random.Range(0, _actionClips.Length);
            AudioSystem.PlaySFX(_actionClips[clipIndex]);
        }
    }
}
