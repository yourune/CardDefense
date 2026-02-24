using System;
using BG_Games.Card_Game_Core.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Items
{
    public class DeckHolder:MonoBehaviour
    {
        [SerializeField] protected TMP_Text NameField;
        [SerializeField] protected Button Button;
        [Space] 
        [SerializeField] protected TMP_Text FullnessText;
        [SerializeField] protected Button DeleteButton;

        protected DeckData Deck;

        public event Action<DeckData> OnDeckSelected;
        public event Action<DeckData> OnDeckDelete;

        private void Awake()
        {
            Button.onClick.AddListener(SelectDeck);
            DeleteButton.onClick.AddListener(DeleteDeck);
        }

        public void InitInfo(DeckData deck)
        {
            Deck = deck;
            NameField.text = deck.Name;
            FullnessText.text = DeckData.GetFullnesAsString(deck);
        }

        protected virtual void DeleteDeck()
        {
            OnDeckDelete?.Invoke(Deck);
        }

        protected virtual void SelectDeck()
        {
            OnDeckSelected?.Invoke(Deck);
        }

    }
}
