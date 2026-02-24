using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Factories;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    public class HeroSelect : MonoBehaviour
    {
        [SerializeField] private RectTransform _screen;        
        [SerializeField] private Button _back;
        [SerializeField] private LayoutGroup _heroesContainer;
        [SerializeField] private HeroPreview _heroPreview;

        private CardLoader _cardLoader;
        private HeroControllerFactory _factory;

        private Action<CardInfo> _selectedCallback;
        private Action _cancelCallback;
        private IList<HeroCardInfo> _heroes;

        private CardInfo _selectedHero;

        [Inject]
        private void Construct(CardLoader cardLoader, HeroControllerFactory factory)
        {
            _cardLoader = cardLoader;
            _factory = factory;
        }

        private void Awake()
        {
            _back.onClick.AddListener(Cancel);
            _heroPreview.OnChooseClick += () => AcceptSelect(_selectedHero);
        }

        public async void Open(Action<CardInfo> selectedCallback, Action cancelCallback)
        {
            if (_heroes == null)
            {
                _heroes = await _cardLoader.LoadHeroes();
                DrawHeroes();
            }

            _selectedCallback = selectedCallback;
            _cancelCallback = cancelCallback;
            _screen.gameObject.SetActive(true);
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
            _selectedCallback = null;
        }

        private void Cancel()
        {
            _cancelCallback?.Invoke();
            Close();
        }

        private void DrawHeroes()
        {
            foreach (var hero in _heroes)
            {
                CardController cardSelect = _factory.Create(hero, _heroesContainer.transform);
                cardSelect.OnClick += hero => SelectHero(hero.CardInfoHolder.Info);
            }
        }

        private void SelectHero(CardInfo hero)
        {
            _selectedHero = hero;

            HeroCardInfo heroInfo = hero as HeroCardInfo;
            _heroPreview.SelectHero(heroInfo);
        }

        private void AcceptSelect(CardInfo hero)
        {
            _selectedCallback?.Invoke(hero);
            Close();
        }
    }
}