using BG_Games.Card_Game_Core.Cards;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    class DeckAssemblyNavigation:MonoBehaviour
    {
        [SerializeField] private RectTransform _screen;

        private DeckCreatorController _creatorController;
        private CollectionController _collectionController;
        private DeckSelect _deckSelect;

        [Inject]
        private void Construct(DeckCreatorController creatorController,CollectionController collectionController,DeckSelect deckSelect)
        {
            _creatorController = creatorController;
            _collectionController = collectionController;
            _deckSelect = deckSelect;
        }

        public void CloseAll()
        {
            _screen.gameObject.SetActive(false);
        }

        public void OpenAll()
        {
            _screen.gameObject.SetActive(true);
        }

        public void OpenDeck(DeckData deck)
        {
            _collectionController.DeckAssemblyStarted(deck);
            _creatorController.Open(deck);
            _deckSelect.Close();
        }

        public void OpenDeckSelect()
        {
            _creatorController.Close();
            _collectionController.EndDeckAssembly();
            _deckSelect.Open();
        }
    }
}
