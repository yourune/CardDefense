using BG_Games.Card_Game_Core.Systems.Localization;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    public class LocalizationInstaller: MonoInstaller
    {
        [SerializeField] private LocalizationSystem _localizationSystem;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalizationSystem>().FromInstance(_localizationSystem).AsSingle();
        }
    }
}