using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Factories
{
    public class CardBarControllerFactory
    {
        private IInstantiator _instantiator;
        private CardBarController _prefab;

        public CardBarControllerFactory(IInstantiator instantiator, CardBarController prefab)
        {
            _instantiator = instantiator;
            _prefab = prefab;
        }

        public CardBarController Create(CardInfo info, Transform parent)
        {
            CardBarController instance = _instantiator.InstantiatePrefabForComponent<CardBarController>(_prefab,parent);
            instance.InitInfo(info);

            return instance;
        }
    }
}
