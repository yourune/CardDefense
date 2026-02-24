using BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters;
using TMPro;
using UnityEngine;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    class CollectionSearchController:MonoBehaviour
    {
        [SerializeField] private CollectionController _collectionController;
        [Space]
        [SerializeField] private TMP_InputField _inputField;

        private NameFilter _filter;

        private void Awake()
        {
            _filter = new NameFilter();

            _inputField.onValueChanged.AddListener(InputChanged);
            _inputField.onSelect.AddListener(InputStarted);
        }

        public void ResetSearch()
        {
            _inputField.text = "";
            _collectionController.SetSearchFilter(null);
        }

        private void InputStarted(string input)
        {
            _collectionController.SetSearchFilter(_filter);
        }

        private void InputChanged(string input)
        {
            _filter.KeyWord = input;
            _collectionController.UpdateView();
        }
    }
}
