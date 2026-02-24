using System.Collections;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    class ActionsDisplay:MonoBehaviour
    {
        [SerializeField] private float _showingTime = 2;
        [SerializeField] private Transform _showPlace;
        [Header("Animation")]
        [SerializeField] private Transform _startPlace;
        [SerializeField] private float _moveTime = 0.5f;
        [SerializeField] private float _startScale = 0.2f;
        [SerializeField] private float _scaleDelay = 0.1f;
        [SerializeField] private float _scaleTime = 0.4f;
        [Space]
        [SerializeField] private UnitInfoHolder _unitCardPreview;
        [SerializeField] private CardInfoHolder _spellCardPreview;

        private float _defaultUnitScale;
        private float _defaultSpellScale;

        private GameObject _currentCard;
        private Coroutine _showingCard;
        
        private Coroutine _moveCoroutine;
        private Coroutine _scaleCoroutine;
        
        private float _defaultScale;

        private PlayerHand _playerHand;

        [Inject]
        private void Construct(PlayerHand playerHand)
        {
            _playerHand = playerHand;
        }

        private void Awake()
        {
            _defaultUnitScale = _unitCardPreview.transform.localScale.x;
            _defaultSpellScale = _spellCardPreview.transform.localScale.x;

            _playerHand.OnCardPlayed += ShowPlayedCard;
        }

        private void ShowPlayedCard(Cards.Controllers.Card card)
        {
            ShowPlayedCard(card.Info);
        }

        public void ShowPlayedCard(CardInfo playedCard)
        {
            StopShowingCard();

            if (playedCard is UnitCardInfo)
            {
                _defaultScale = _defaultUnitScale;
                _unitCardPreview.InitInfo(playedCard);
                _showingCard = StartCoroutine(ShowCard(_unitCardPreview));
            }
            else if(playedCard is SpellCardInfo)
            {
                _defaultScale = _defaultSpellScale;
                _spellCardPreview.InitInfo(playedCard);
                _showingCard = StartCoroutine(ShowCard(_spellCardPreview));
            }
        }

        private IEnumerator ShowCard(CardInfoHolder preview)
        {
            preview.SetMode(CardViewMode.Front);
            preview.gameObject.SetActive(true);

            AnimateShow(preview);

            _currentCard = preview.gameObject;

            yield return new WaitForSeconds(_showingTime);
            StopShowingCard();
        }

        private void StopShowingCard()
        {
            if (_showingCard != null)
            {
                StopCoroutine(_showingCard);
                _showingCard = null;
            }

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            if (_scaleCoroutine != null)
            {
                StopCoroutine(_scaleCoroutine);
                _scaleCoroutine = null;
            }

            if (_currentCard != null)
            {
                _currentCard.transform.localScale = Vector3.one * _defaultScale;
                _currentCard.SetActive(false);
                _currentCard = null;
            }
        }

        private async void AnimateShow(CardInfoHolder preview)
        {
            if (preview == null) return;

            preview.transform.localScale = Vector3.one * _startScale;
            preview.transform.position = _startPlace.position;
            
            _moveCoroutine = StartCoroutine(TweenAnimation.MoveTo(
                preview.transform,
                _showPlace.position,
                _moveTime,
                TweenAnimation.EaseFlash
            ));

            await UniTask.Delay((int)(_scaleDelay * 1000));

            if (preview == null) return;
            
            _scaleCoroutine = StartCoroutine(TweenAnimation.ScaleTo(
                preview.transform,
                Vector3.one * _defaultScale,
                _scaleTime,
                TweenAnimation.EaseOutElastic
            ));
        }
    }
}
