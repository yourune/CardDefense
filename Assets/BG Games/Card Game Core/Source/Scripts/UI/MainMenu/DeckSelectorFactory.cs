using BG_Games.Card_Game_Core.Cards;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    class DeckSelectorFactory:MonoBehaviour
    {
        [SerializeField] private DeckSelector _prefab;

        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public DeckSelector Create(DeckData deck, Transform parent)
        {
            DeckSelector instance = _instantiator.InstantiatePrefabForComponent<DeckSelector>(_prefab.gameObject, parent);
            instance.InitInfo(deck);

            return instance;
        }
    }
}
