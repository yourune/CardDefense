using BG_Games.Card_Game_Core.Cards;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    class PreMatchDeckSelect : MonoBehaviour
    {
        [SerializeField] private AIDeckSelect _aiDeckSelect;
        [SerializeField] private PlayerDeckSelect _playerDeckSelect;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _screen;

        private MainMenuController _menu;

        [Inject]
        private void Construct(MainMenuController menu)
        {
            _menu = menu;
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(Play);
            _closeButton.onClick.AddListener(Close);
            _playerDeckSelect.OnSelectedDeck += UpdateDeck;
            _aiDeckSelect.OnSelectedDeck += UpdateDeck;
        }

        public void Open()
        {
            _screen.gameObject.SetActive(true);
            _playerDeckSelect.UpdateView();
            _aiDeckSelect.UpdateView();
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
            _menu.Open();
        }

        private void Play()
        {
            _aiDeckSelect.SaveChoose();
            _playerDeckSelect.SaveChoose();
            _menu.DeckSelected();
        }

        private void UpdateDeck(DeckData deckData)
        {
            UpdatePlayView();
        }

        private void UpdatePlayView()
        {
            _playButton.gameObject.SetActive(_playerDeckSelect.HasSelected && _aiDeckSelect.HasSelected);
        }
    }
}