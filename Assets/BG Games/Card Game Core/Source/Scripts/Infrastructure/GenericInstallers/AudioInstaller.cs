using BG_Games.Card_Game_Core.Systems.Audio;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    class AudioInstaller:MonoInstaller
    {
        [SerializeField] private AudioSystem _audioSystem;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioSystem>().FromInstance(_audioSystem).AsSingle();
        }
    }
}
