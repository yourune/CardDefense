using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Systems;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace BG_Games.Card_Game_Core.AI
{
    class AIDeckProvider:DeckProvider
    {
        [SerializeField] private AssetLabelReference _aiDecksLabel;
        
        private DeckSelectUtility _deckSelectUtility;

        [Inject]
        private void Construct(DeckSelectUtility selectUtility)
        {
            _deckSelectUtility = selectUtility;
        }

        private async UniTask<IList<TextAsset>> LoadDecksByLabel(AssetLabelReference label)
        {
            AsyncOperationHandle<IList<TextAsset>> handle = Addressables.LoadAssetsAsync<TextAsset>(label, null);
            await handle.ToUniTask(PlayerLoopTiming.PostLateUpdate);

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<TextAsset> decks = handle.Result;

                Addressables.Release(handle);
                return decks;
            }
            else
            {
                throw new Exception($"Failed to load deck assets with label '{label.labelString}'");
            }
        }

        public override async UniTask<DeckData> GetDeck()
        {
            IList<TextAsset> decks = await LoadDecksByLabel(_aiDecksLabel);

            if (decks.Count == 0)
            {
                throw new Exception("There are no decks pre-built for AI");
            }

            string deckName = _deckSelectUtility.GetAISelectedDeck();
            
            // If no AI deck is selected or it doesn't exist, use the first available deck
            if (string.IsNullOrEmpty(deckName))
            {
                DeckData firstDeck = JsonUtility.FromJson<DeckData>(decks[0].text);
                Debug.LogWarning($"No AI deck selected. Automatically selecting '{firstDeck.Name}'.");
                return firstDeck;
            }

            // Try to find the selected deck
            foreach (var deck in decks)
            {
                DeckData result = JsonUtility.FromJson<DeckData>(deck.text);
                
                if (result.Name == deckName)
                {
                    return result;
                }
            }
            
            // If selected deck wasn't found, use the first available deck
            DeckData fallbackDeck = JsonUtility.FromJson<DeckData>(decks[0].text);
            string availableDeckNames = string.Join(", ", decks.Select(d => JsonUtility.FromJson<DeckData>(d.text).Name));
            Debug.LogWarning($"AI deck '{deckName}' not found. Automatically selecting '{fallbackDeck.Name}'. Available AI decks: {availableDeckNames}");
            
            return fallbackDeck;
        }
    }
}
