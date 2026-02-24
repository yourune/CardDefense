using BG_Games.Card_Game_Core.Cards;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    class DeckSelectUtility
    {
        public const string SelectedDeckKey = "SelectedDeck";
        public const string AISelectedDeckKey = "AISelectedDeck";

        public void SetSelectedDeck(DeckData deck)
        {
            PlayerPrefs.SetString(SelectedDeckKey,deck.Name);
        }

        public string GetSelectedDeck()
        {
            return PlayerPrefs.GetString(SelectedDeckKey);
        }
        
        public void SetAISelectedDeck(DeckData deck)
        {
            PlayerPrefs.SetString(AISelectedDeckKey,deck.Name);
        }

        public string GetAISelectedDeck()
        {
            return PlayerPrefs.GetString(AISelectedDeckKey);
        }
    }
}
