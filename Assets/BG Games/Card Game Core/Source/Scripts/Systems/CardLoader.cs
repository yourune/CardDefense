using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    [Serializable]
    struct RaceLabel
    {
        public CardRace Race;
        public AssetLabelReference Label;
    }

    public class CardLoader:MonoBehaviour
    {
        [SerializeField] private AssetLabelReference _cardsLabel;
        [SerializeField] private AssetLabelReference _heroLabel;
        [SerializeField] private RaceLabel[] _raceLabels;

        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        public async UniTask<IList<HeroCardInfo>> LoadHeroes()
        {
            return await LoadHeroesByLabel(_heroLabel);
        }

        public async UniTask<IList<CardInfo>> LoadByRace(CardRace race)
        {
            AssetLabelReference label = _raceLabels.First(label => label.Race == race).Label;
            return await LoadCardsByLabel(label);

        }

        public async UniTask<IList<CardInfo>> LoadAllCards()
        {
            return await LoadCardsByLabel(_cardsLabel);
        }

        public async UniTask<CardInfo> LoadCard(string id)
        {
            return await LoadCardByKey(id);
        }

        public async UniTask<IList<CardInfo>> LoadDeckCards(DeckData deck)
        {
            List<string> cards = new List<string>();
            foreach (var id in deck.Cards)
            {
                if (!string.IsNullOrEmpty(id))
                {
                   cards.Add(id); 
                }
            }

            if (cards.Count > 0)
            {
                return await LoadCardsByKeys(cards);
            }

            return null;
        }


        private async UniTask<IList<HeroCardInfo>> LoadHeroesByLabel(AssetLabelReference label)
        {
            AsyncOperationHandle<IList<HeroCardInfo>> handle = Addressables.LoadAssetsAsync<HeroCardInfo>(label, null);
            await handle.ToUniTask(PlayerLoopTiming.PostLateUpdate);

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<HeroCardInfo> cards = handle.Result;

                //Addressables.Release(handle);
                return cards;
            }
            else
            {
                throw new Exception($"Failed to load card assets with label '{label}'");
            }
        }

        private async UniTask<IList<CardInfo>> LoadCardsByLabel(AssetLabelReference label)
        {
            AsyncOperationHandle<IList<CardInfo>> handle = Addressables.LoadAssetsAsync<CardInfo>(label, null);
            await handle.ToUniTask(PlayerLoopTiming.PostLateUpdate);

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<CardInfo> cards = handle.Result;

                //Addressables.Release(handle);
                return cards;
            }
            else
            {
                throw new Exception($"Failed to load card assets with label '{label}'");
            }
        }

        private async UniTask<IList<CardInfo>> LoadCardsByKeys(IEnumerable<string> keys)
        {
            AsyncOperationHandle<IList<CardInfo>> handle = Addressables.LoadAssetsAsync<CardInfo>(keys, null, Addressables.MergeMode.Union);
            await handle.ToUniTask(PlayerLoopTiming.PostLateUpdate);


            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<CardInfo> resultCards = handle.Result;
                
                //Addressables.Release(handle);
                return resultCards;
            }
            else
            {
                throw new Exception($"Failed to load card assets");
            }
        }

        private async UniTask<CardInfo> LoadCardByKey(string key)
        {
            AsyncOperationHandle<CardInfo> handle = Addressables.LoadAssetAsync<CardInfo>(key);
            await handle.ToUniTask(PlayerLoopTiming.PostLateUpdate);

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                CardInfo info = handle.Result;

                //Addressables.Release(handle);
                return info;
            }
            else
            {
                throw new Exception($"Failed to load card asset with key '{key}'");
            }
        }
    }
}
