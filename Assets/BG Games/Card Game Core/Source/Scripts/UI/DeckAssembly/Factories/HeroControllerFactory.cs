using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Factories
{
    class HeroControllerFactory:MonoBehaviour
    {
        [SerializeField] private CardController _prefab;

        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public CardController Create(HeroCardInfo info, Transform parent)
        {
            CardController instance = _instantiator.InstantiatePrefabForComponent<CardController>(_prefab.gameObject, parent);
            instance.InitInfo(info);
            return instance;
        }
    }
}
