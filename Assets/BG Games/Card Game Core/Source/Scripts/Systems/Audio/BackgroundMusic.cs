using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems.Audio
{
    class BackgroundMusic:MonoBehaviour
    {
        [SerializeField]private AudioClip _music;
        
        private AudioSystem _audioSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
        }

        private void Start()
        {
            _audioSystem.SetMusic(_music);
        }

    }
}
