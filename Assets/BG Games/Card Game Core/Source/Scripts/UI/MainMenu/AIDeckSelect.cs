using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    class AIDeckSelect : DeckSelectBase
    {
        [SerializeField] private AssetLabelReference _labelAI;
        
        private DeckData[] _decksAI;
        
        private DeckSelectUtility _deckSelectUtility;

        [Inject]
        private void Construct(DeckSelectUtility deckSelectUtility)
        {
            _deckSelectUtility = deckSelectUtility;
        }

        protected override async UniTask InitAsync()
        {
            _decksAI = await GetAIDecks(_labelAI);
            await base.InitAsync();
        }
        
        protected override void UpdateDeckHolders()
        {
            base.UpdateDeckHolders();
            ClearDeckHolders();
            
            foreach (DeckData deck in _decksAI)
            {
                AddDeckSelector(deck);
            }
            
            UpdateView();
        }

        protected override async void UpdateDeckView(DeckSelector selector, DeckData deck)
        {
            base.UpdateDeckView(selector, deck);
            selector.SetActiveCount(false);
            
            if (Heroes == null)
            {
                await InitAsyncCompletionSource.Task;
            }

            HeroCardInfo deckHero = Heroes.First(hero => hero.Id == deck.Hero);
            
            selector.SetName(deckHero.Name.GetLocalizedString());
        }

        public override void SaveChoose()
        {
            base.SaveChoose();
            _deckSelectUtility.SetAISelectedDeck(SelectedDeck);
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

        private async UniTask<DeckData[]> GetAIDecks(AssetLabelReference label)
        {
            IList<TextAsset> decksText = await LoadDecksByLabel(label);
            DeckData[] decks = new DeckData[decksText.Count];

            for (int i = 0; i < decks.Length; i++)
            {
                decks[i] = JsonUtility.FromJson<DeckData>(decksText[i].text);
            }

            return decks;
        }
    }
}
