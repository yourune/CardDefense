using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Factories
{
    public class DeckNameHolderFactory
    {
        private IInstantiator _instantiator;
        private DeckHolder _prefab;

        public DeckNameHolderFactory(IInstantiator instantiator, DeckHolder prefab)
        {
            _instantiator = instantiator;
            _prefab = prefab;
        }

        public DeckHolder Create(DeckData deck, Transform parent)
        {
            DeckHolder instance = _instantiator.InstantiatePrefabForComponent<DeckHolder>(_prefab.gameObject,parent);
            instance.InitInfo(deck);

            return instance;
        }
    }
}
