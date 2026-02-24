using BG_Games.Card_Game_Core.UI.DeckAssembly;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Factories;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.SceneDependentInstallers
{
    public class DeckAssemblyInstaller : MonoInstaller
    {
        [Header("Factories")] 
        [SerializeField] private CardControllerFactory _cardControllerFactory;
        [SerializeField] private HeroControllerFactory _heroControllerFactory;

        [Header("Deck sellect")] 
        [SerializeField] private DeckSelect _deckSelect;
        [SerializeField] private HeroSelect _heroSelect;
        [Space] 
        [SerializeField] private DeckHolder _deckHolderPrefab;
        
        [Header("Deck assembly")]
        [SerializeField] private DeckAssemblyNavigation _deckAssemblyNavigation;
        [SerializeField] private CollectionController _collectionController;
        [SerializeField] private DeckCreatorController _deckCreatorController;
        [Space]
        [SerializeField] private CardBarController _cardBarPrefab;


        public override void InstallBindings()
        {
            InstallAssemblyControllers();
            InstallSelectControllers();
            InstallCollection();

            InstallBarControllerFactory();
            InstallControllerFactory();
            InstallDeckHolderFactory();
        }

        private void InstallAssemblyControllers()
        {            
            Container.Bind<DeckAssemblyNavigation>().FromInstance(_deckAssemblyNavigation).AsSingle();            
            Container.Bind<DeckCreatorController>().FromInstance(_deckCreatorController).AsSingle();
        }

        private void InstallCollection()
        {
            Container.Bind<CollectionController>().FromInstance(_collectionController).AsSingle();
        }

        private void InstallSelectControllers()
        {
            Container.Bind<DeckSelect>().FromInstance(_deckSelect).AsSingle();
            Container.Bind<HeroSelect>().FromInstance(_heroSelect).AsSingle();
        }

        private void InstallDeckHolderFactory()
        {
            Container.Bind<DeckHolder>().FromInstance(_deckHolderPrefab).AsSingle();
            Container.Bind<DeckNameHolderFactory>().FromNew().AsSingle();
        }

        private void InstallBarControllerFactory()
        {
            Container.Bind<CardBarController>().FromInstance(_cardBarPrefab).AsSingle();
            Container.Bind<CardBarControllerFactory>().FromNew().AsSingle();
        }

        private void InstallControllerFactory()
        {
            Container.Bind<CardControllerFactory>().FromInstance(_cardControllerFactory).AsSingle();
            Container.Bind<HeroControllerFactory>().FromInstance(_heroControllerFactory).AsSingle();
        }
    }
}