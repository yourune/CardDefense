using BG_Games.Card_Game_Core.Systems.Audio;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Audio
{
    public class HeroAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip _click;
        [SerializeField] private AudioClip _unacces;

        private AudioClip _effect;
        
        private AudioSystem _audioSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
        }

        public void Init(Hero card)
        {
           card.HeroLogic.OnApply += EffectSound;
           _effect = card.HeroInfo.SFX;
        }

        public void ClickSound()
        {
            _audioSystem.PlayUI(_click);
        }

        public void UnaccessSound()
        {
            _audioSystem.PlayUI(_unacces);
        }

        private void EffectSound()
        {
            if (_effect != null)
            {
                _audioSystem.PlaySFX(_effect);
            }
        }
    }
}
