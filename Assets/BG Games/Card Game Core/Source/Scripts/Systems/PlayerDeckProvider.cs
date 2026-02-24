using System;
using BG_Games.Card_Game_Core.Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    class PlayerDeckProvider:DeckProvider
    {
        private DeckData _deck;
        private DeckSelectUtility _deckSelectUtility;
        private IDeckSaveSystem _deckSaveSystem;

        [Inject]
        private void Construct(DeckSelectUtility selectUtility,IDeckSaveSystem deckSaveSystem)
        {
            _deckSelectUtility = selectUtility;
            _deckSaveSystem = deckSaveSystem;
        }

        public override UniTask<DeckData> GetDeck()
        {
            string selectedDeck = _deckSelectUtility.GetSelectedDeck();
            string[] availableDecks = _deckSaveSystem.LoadDeckNames();

            // If no deck is selected or selected deck doesn't exist, try to use the first available deck
            if (string.IsNullOrEmpty(selectedDeck) || !System.Array.Exists(availableDecks, name => name == selectedDeck))
            {
                if (availableDecks.Length > 0)
                {
                    selectedDeck = availableDecks[0];
                    Debug.LogWarning($"No valid deck selected. Automatically selecting '{selectedDeck}'.");
                }
                else
                {
                    throw new Exception("No decks are available. Please create a deck before starting the game.");
                }
            }

            if (_deckSaveSystem.LoadDeck(selectedDeck, out _deck) == false)
            {
                throw new Exception($"Deck '{selectedDeck}' could not be loaded. Available decks: {string.Join(", ", availableDecks)}");
            }

            UniTask<DeckData> task = UniTask.FromResult<DeckData>(_deck);

            return task;
        }
    }
}
