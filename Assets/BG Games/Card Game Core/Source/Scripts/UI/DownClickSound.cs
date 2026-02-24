using BG_Games.Card_Game_Core.Systems.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BG_Games.Card_Game_Core.UI
{
    class DownClickSound:MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private AudioClip _sound;

        private AudioSystem _audioSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _audioSystem.PlayUI(_sound);
        }
    }
}
