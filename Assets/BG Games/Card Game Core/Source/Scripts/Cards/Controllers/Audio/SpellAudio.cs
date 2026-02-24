using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Audio
{
    class SpellAudio:CardAudio
    {
        private SpellCardInfo _spellInfo;
        private AudioClip _effectSound;

        public override void Init(Card card)
        {
            base.Init(card);

            if (card is SpellCard spellCard)
            {
                _spellInfo = spellCard.SpellInfo;
                spellCard.SpellLogic.OnApply += EffectSound;
            }

            InstantiateSounds();
        }

        private void InstantiateSounds()
        {
            if (_spellInfo != null && _spellInfo.SFX != null)
            {
                _effectSound = _spellInfo.SFX;
            }
        }

        private void EffectSound()
        {
            if (_effectSound != null)
            {
                AudioSystem.PlaySFX(_effectSound);
            }
        }

    }
}
