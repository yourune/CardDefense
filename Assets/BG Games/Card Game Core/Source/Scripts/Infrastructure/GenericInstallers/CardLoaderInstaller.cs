using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    public class CardLoaderInstaller : MonoInstaller
    {
        [SerializeField] private CardLoader _cardLoader;

        public override void InstallBindings()
        {
            Container.Bind<CardLoader>().FromInstance(_cardLoader).AsSingle();
        }
    }
}