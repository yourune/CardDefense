using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Factories
{
    class HeroCardFactory
    {
        private Hero _hero;

        private DiContainer _container;
        private InputLockSystem _inputLockSystem;

        private HeroCardFactory(DiContainer container,InputLockSystem inputLockSystem,CardFactoriesSettings settings)
        {
            _container = container;
            _inputLockSystem = inputLockSystem;
            _hero = settings.HeroPrefab;
        }

        public Hero Create(CardInfo info,Player.Player owner)
        {
            HeroCardInfo heroInfo = info as HeroCardInfo;
            _container.Inject(heroInfo.LogicFactory);

            Hero instance = _container.InstantiatePrefabForComponent<Hero>(_hero, owner.CardsParent);
            instance.InitInfo(heroInfo);

            _inputLockSystem.AddInputReader(instance);
            return instance;
        }
    }
}
