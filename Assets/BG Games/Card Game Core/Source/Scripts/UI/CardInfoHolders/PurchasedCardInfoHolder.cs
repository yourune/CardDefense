using System.Collections;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.CurrencySystem;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.CardInfoHolders
{
    public class PurchasedCardInfoHolder : CardInfoHolder
    {
        [Header("Purchased")] 
        [SerializeField] private PurchasedCardInfoPanel _panelPrefab;
        [SerializeField] private RectTransform _panelContainer;

        private IPlayerInventorySaveSystem _inventory;
        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IPlayerInventorySaveSystem inventory, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _inventory = inventory;
        }

        public override void InitInfo(CardInfo info)
        {
            base.InitInfo(info);
            if(!(info is DeckCardInfo deckCardInfo)) 
                return;
            
            if (_panelPrefab == null || _inventory.ContainsCardId(info.Id))
                return;

            PurchasedCardInfoPanel panel =
                _instantiator.InstantiatePrefabForComponent<PurchasedCardInfoPanel>(_panelPrefab.gameObject,
                    _panelContainer);
            panel.InitInfo(deckCardInfo);
        }
    }
}
