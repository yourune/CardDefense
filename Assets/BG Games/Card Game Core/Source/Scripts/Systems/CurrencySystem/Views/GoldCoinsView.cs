using System;
using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using TMPro;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems.CurrencySystem.Views
{
    public class GoldCoinsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;

        private ICurrencyService _currencyService;
        
        [Inject]
        private void Construct(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        
        private void Awake()
        {
            EventBus.Subscribe<GoldCoinsUpdateEvent>(OnUpdateCoins);
        }

        private void OnEnable()
        {
            UpdateCoins(_currencyService.GoldCoins);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<GoldCoinsUpdateEvent>(OnUpdateCoins);
        }

        private void OnUpdateCoins(GoldCoinsUpdateEvent data)
        {
            UpdateCoins(data.GoldCoins);
        }

        private void UpdateCoins(int coins)
        {
            _coinsText.text = coins.ToString();
        }
    }
}
