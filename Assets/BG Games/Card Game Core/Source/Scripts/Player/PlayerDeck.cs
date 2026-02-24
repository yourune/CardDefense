using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Factories;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.Audio;
using BG_Games.Card_Game_Core.Tools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BG_Games.Card_Game_Core.Player
{
    public class PlayerDeck : MonoBehaviour
    {
        [SerializeField] private AudioClip _dealSound;
        [SerializeField] private AudioClip _dealSeveralSound;
        [Space]
        [SerializeField] private SpriteRenderer _cards;

        private UniTaskCompletionSource<DeckData> _deckLoadCompletionSource;
        private UniTaskCompletionSource<DeckData> DeckLoadCompletionSource
        {
            get
            {
                if (_deckLoadCompletionSource == null)
                {
                    _deckLoadCompletionSource = new UniTaskCompletionSource<DeckData>();
                }

                return _deckLoadCompletionSource;
            }
        }

        private DeckData _deck;

        public List<int> DrawOrder { get; private set; }

        private List<CardInfo> _deckCards;
        private IList<CardInfo> _cardsSource;
        private CardInfo _heroInfo;

        private AudioSystem _audioSystem;
        private CardFactory _cardFactory;
        private HeroCardFactory _heroCardFactory;
        private CardLoader _loader;
        private Player _player;
        private DeckProvider _deckProvider;
        private CardViewSettings _cardsViewSettings;

        public bool LeftCards => DrawOrder.Count > 0;
        public int LeftCardsNumber => DrawOrder.Count;

        [Inject]
        private void Construct(CardFactory cardFactory,HeroCardFactory heroCardFactory, CardLoader loader, Player player,DeckProvider deckProvider, CardViewSettings viewSettings, AudioSystem audioSystem)
        {
            _cardFactory = cardFactory;
            _heroCardFactory = heroCardFactory;
            _deckProvider = deckProvider;
            _cardsViewSettings = viewSettings;
            _audioSystem = audioSystem;

            _loader = loader;
            _player = player;
        }

        protected virtual async void Awake()
        {
            _deck = await _deckProvider.GetDeck();

            _cards.sprite = _cardsViewSettings.GetBackViewOfDeck(_deck);
            DeckLoadCompletionSource.TrySetResult(_deck);
        }

        public async UniTask<Hero> GetHero()
        {
            if (_heroInfo == null)
            {
                await DeckLoadCompletionSource.Task;

                _heroInfo = await _loader.LoadCard(_deck.Hero);
            }

            return _heroCardFactory.Create(_heroInfo, _player);
        }

        public async UniTask<Card> DrawCard(bool sound = true)
        {
            if (_cardsSource == null)
            {
                await LoadDeckCards();
                InitDraw();
            }

            int index = DrawOrder.First();
            CardInfo card = _deckCards[index];
            DrawOrder.RemoveAt(0);
            
            UpdateVisual();

            if (sound)
                _audioSystem.PlaySFX(_dealSound);

            return _cardFactory.Create(card,_player);
        }

        public async UniTask<Card[]> DrawCards(int amount)
        {
            Card[] result = new Card[amount];

            if (_cardsSource == null)
            {
                await LoadDeckCards();
                InitDraw();
            }

            for (int i = 0; i < amount; i++)
            {
                result[i] = await DrawCard(false);
            }

            if (_player.PlayCardInputSounds)
            {
                _audioSystem.PlaySFX(_dealSeveralSound);
            }

            return result;
        }

        public void ReturnCardToEnd(CardInfo card)
        {
            int index = _deckCards.IndexOf(card);
            DrawOrder.Add(index);

            UpdateVisual();
        }

        public void ReturnCardToRandom(CardInfo card)
        {
            int index = _deckCards.IndexOf(card);
            int position = Random.Range(0, DrawOrder.Count + 1);
            DrawOrder.Insert(position,index);

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            _cards.enabled = LeftCards;
        }

        private async UniTask LoadDeckCards()
        {
            await DeckLoadCompletionSource.Task;

            _cardsSource = await _loader.LoadDeckCards(_deck);
        }

        private void InitDraw()
        {
            if (_deckCards == null)
            {
                _deckCards = new List<CardInfo>();
                DrawOrder = new List<int>();

                for (int i = 0; i < _deck.Cards.Length; i++)
                {
                    CardInfo info = _cardsSource.First(card => card.Id == _deck.Cards[i]);
                    _deckCards.Add(info);
                    DrawOrder.Add(i);
                }

                DrawOrder.Shuffle();
            }
        }
    }
}