using BG_Games.Card_Game_Core.Systems.Audio;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Audio
{
    public class CardAudio:MonoBehaviour
    {
        [SerializeField] protected AudioClip _clickSound;
        [SerializeField] protected AudioClip _placeSound;
        [SerializeField] protected AudioClip _unaccessSound;
        [SerializeField] protected AudioClip _returnSound;


        protected AudioSystem AudioSystem;

        [Inject]
        private void Construct(AudioSystem system)
        {
            AudioSystem = system;
        }

        public virtual void Init(Card card)
        {

        }


        protected virtual void Start()
        {

        }

        public void ClickSound()
        {
            AudioSystem.PlayUI(_clickSound);
        }

        public void PlaceSound()
        {
            AudioSystem.PlayUI(_placeSound);
        }

        public void UnaccessSound()
        {
            AudioSystem.PlayUI(_unaccessSound);
        }

        public void ReturnSound()
        {
            AudioSystem.PlayUI(_returnSound);
        }
    }
}
