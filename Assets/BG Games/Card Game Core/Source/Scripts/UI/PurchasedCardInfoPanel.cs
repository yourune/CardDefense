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


namespace BG_Games.Card_Game_Core.UI
{
    public class PurchasedCardInfoPanel : MonoBehaviour
    {
        [SerializeField] private LocalizedString _priceLocalized;
        [SerializeField] private RectTransform _lockPanel;
        [SerializeField] private RectTransform _payPanel;
        [SerializeField] private Button _lockButton;
        [SerializeField] private Button _yesBuyButton;
        [SerializeField] private Button _noBuyButton;
        [SerializeField] private TMP_Text _priceText;

        private ICurrencyService _currencyService;
        private IPlayerInventorySaveSystem _inventory;
        private Coroutine _waitClick;
        private DeckCardInfo _info;
        
        [Inject]
        private void Construct(ICurrencyService currencyService, IPlayerInventorySaveSystem inventory)
        {
            _currencyService = currencyService;
            _inventory = inventory;
        }
        
        private void Awake()
        {
            _lockButton.onClick.AddListener(OpenPayPanel);
            _yesBuyButton.onClick.AddListener(OnYesBuyClick);
            _noBuyButton.onClick.AddListener(OnNoBuyClick);
        }

        public void InitInfo(DeckCardInfo info)
        {
            _info = info;
            _lockPanel.gameObject.SetActive(true);
            _priceText.text = _priceLocalized.GetLocalizedString().FormatSmart(info.GoldPrice);
        }

        private void OpenPayPanel()
        {
            SetActivePayPanel(true);
        }

        private void OnYesBuyClick()
        {
            SetActivePayPanel(false);

            if (_currencyService.TryPayGoldCoins(_info.GoldPrice))
            {
                _inventory.AddCardId(_info.Id);
                _lockPanel.gameObject.SetActive(false);
            }
        }

        private void OnNoBuyClick()
        {
            SetActivePayPanel(false);
        }

        private void SetActivePayPanel(bool value)
        {
            _payPanel.gameObject.SetActive(value);
            _lockButton.gameObject.SetActive(!value);

            if (value)
                StartWaitClick();
        }

        private IEnumerator WaitClick()
        {
            while (true)
            {
                yield return null;
                if (Input.GetMouseButtonUp(0) || Input.touchCount > 0)
                {
                    SetActivePayPanel(false);
                    break;
                }
            }
        }

        private void StartWaitClick()
        {
            StopWaitClick();
            _waitClick = StartCoroutine(WaitClick());
        }

        private void StopWaitClick()
        {
            if (_waitClick != null)
                StopCoroutine(_waitClick);
        }
    }
}
