using System;
using BG_Games.Card_Game_Core.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    class DeckSelector: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] protected TMP_Text NameField;
        [SerializeField] protected TMP_Text FullnessField;
        [SerializeField] protected Button Button;
        [Space] 
        [SerializeField] private RectTransform _selectState;

        public event Action<DeckSelector> OnDeckClicked;
        public event Action<DeckData> OnDeckSelected;

        public DeckData Deck { get; protected set; }

        private void Awake()
        {
            Button?.onClick.AddListener(SelectDeck);
        }

        public void InitInfo(DeckData deck)
        {
            Deck = deck;
            NameField.text = deck.Name;
            FullnessField.text = DeckData.GetFullnesAsString(deck);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HandleClick();
        }

        public void HandleClick()
        {
            OnDeckClicked?.Invoke(this);
            _selectState.gameObject.SetActive(true);
        }

        public void Unselect()
        {
            _selectState.gameObject.SetActive(false);
        }

        public void SetActiveCount(bool isActive)
        {
            FullnessField.gameObject.SetActive(isActive);
        }

        public void SetName(string deckName)
        {
            NameField.text = deckName;
        }

        protected void SelectDeck()
        {
            OnDeckSelected?.Invoke(Deck);
            Unselect();
        }
    }
}
