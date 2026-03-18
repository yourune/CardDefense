using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CardDefense.Core.Events;

namespace CardDefense.Cards
{
    public class RefactoredHandSystem : MonoBehaviour
    {
        [Header("Deck & Piles")]
        [SerializeField] private RefactoredDeckData _startingDeck; // 인스펙터에서 할당할 덱 데이터
        [SerializeField] private int _initialDrawAmount = 5; // 게임 시작 시 보여줄 드로우 매수
        [SerializeField] private Transform _drawPilePoint;
        [SerializeField] private Transform _discardPilePoint;

        [Header("Grid Layout (5x2)")]
        [SerializeField] private List<Transform> _cardSlots = new List<Transform>(); 
        [SerializeField] private float cardAnimationDuration = 0.2f;

        // 실제 데이터를 들고 있을 카드 덱
        private Queue<RefactoredCardData> _drawPile = new Queue<RefactoredCardData>();
        private List<RefactoredCardData> _discardPile = new List<RefactoredCardData>();

        // 손에 들고 있는 프리팹 객체 목록 (최대 10장)
        public List<RefactoredCardView> currentHand = new List<RefactoredCardView>();

        [Header("Card Setup")]
        [SerializeField] private RefactoredCardView _cardPrefab; // 스폰할 카드 모형

        private void OnEnable()
        {
            EventBus.Subscribe<CardUsedEvent>(OnCardUsed);
            EventBus.Subscribe<TriggerDrawCardEvent>(OnDrawCardRequested);
        }

        private void Start()
        {
            // 인스펙터에 등록된 시작 덱이 있다면 자동으로 초기화 후 초기 카드를 뽑습니다.
            if (_startingDeck != null && _startingDeck.Cards != null && _startingDeck.Cards.Count > 0)
            {
                InitializeDeck(new List<RefactoredCardData>(_startingDeck.Cards));
                if (_initialDrawAmount > 0)
                {
                    DrawCard(_initialDrawAmount);
                }
            }
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<CardUsedEvent>(OnCardUsed);
            EventBus.Unsubscribe<TriggerDrawCardEvent>(OnDrawCardRequested);
        }

        private void OnDrawCardRequested(TriggerDrawCardEvent evt)
        {
            DrawCard(evt.Amount);
        }

        /// <summary>
        /// 게임 시작 시 덱(List<SO>)을 받아와서 DrawPile에 넣습니다.
        /// </summary>
        public void InitializeDeck(List<RefactoredCardData> initialDeck)
        {
            _drawPile.Clear();
            _discardPile.Clear();

            // 간단한 셔플 빙식 (추후 정밀한 셔플 알고리즘으로 교체 가능)
            initialDeck.Sort((a, b) => UnityEngine.Random.Range(-1, 2));

            foreach (var card in initialDeck)
            {
                _drawPile.Enqueue(card);
            }
        }

        /// <summary>
        /// 드로우 파일에서 카드를 뽑아 손(Grid)으로 가져옵니다.
        /// </summary>
        public void DrawCard(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                // 손패가 꽉 찼다면 가장 오래된(1번 슬롯) 카드를 버리고 공간을 확보합니다.
                if (currentHand.Count >= _cardSlots.Count)
                {
                    DiscardLeftmostCard();
                }

                if (_drawPile.Count == 0)
                {
                    RefillDeck();
                    if (_drawPile.Count == 0)
                    {
                        break; 
                    }
                }

                RefactoredCardData drawnData = _drawPile.Dequeue();
                
                if (_cardPrefab == null || _drawPilePoint == null)
                {
                    return;
                }

                // 카드 프리팹 생성 (DrawPile 위치에서)
                RefactoredCardView newCard = Instantiate(_cardPrefab, _drawPilePoint.position, _drawPilePoint.rotation);
                newCard.Setup(drawnData);

                currentHand.Add(newCard);
            }
            
            // 모든 카드를 뽑은 후 슬롯 위치로 재정렬(이동 애니메이션)
            RepositionCards();
        }

        private void RefillDeck()
        {
            _discardPile.Sort((a, b) => UnityEngine.Random.Range(-1, 2));
            foreach(var card in _discardPile)
            {
                _drawPile.Enqueue(card);
            }
            _discardPile.Clear();
        }

        private void OnCardUsed(CardUsedEvent evt)
        {
            // 사용된 카드 객체 찾기
            var cardToRemove = currentHand.Find(c => c.CardID == evt.CardID);
            if (cardToRemove != null)
            {
                currentHand.Remove(cardToRemove);
                
                // 데이터는 Discard Pile로
                _discardPile.Add(cardToRemove.Data);

                // DOTween 에러 방지: 킬 먼저 하고 이동
                cardToRemove.transform.DOKill();
                
                // 버려지는 연출 (Discard pile로 날아가면서 작아지고 파괴)
                cardToRemove.transform.DOMove(_discardPilePoint.position, cardAnimationDuration)
                    .OnComplete(() => {
                        if(cardToRemove != null && cardToRemove.gameObject != null)
                            Destroy(cardToRemove.gameObject);
                    });
                cardToRemove.transform.DOScale(Vector3.zero, cardAnimationDuration);

                // 손패 빈 공간 메꾸기(앞으로 당기기)
                RepositionCards();
            }
        }

        /// <summary>
        /// 특정 카드를 강제로 가장 왼쪽에서부터 버립니다 (오버드로우 또는 특정 효과)
        /// </summary>
        public void DiscardLeftmostCard()
        {
            if (currentHand.Count > 0)
            {
                var cardToDiscard = currentHand[0];
                currentHand.RemoveAt(0);
                
                _discardPile.Add(cardToDiscard.Data);
                
                cardToDiscard.transform.DOKill();
                cardToDiscard.transform.DOMove(_discardPilePoint.position, cardAnimationDuration)
                    .OnComplete(() => {
                        if(cardToDiscard != null && cardToDiscard.gameObject != null)
                            Destroy(cardToDiscard.gameObject);
                    });
                cardToDiscard.transform.DOScale(Vector3.zero, cardAnimationDuration);
                
                RepositionCards();
            }
        }

        // 손패 재정렬 (5x2 그리드 슬롯 기반으로 맨 앞부터 당겨서 채움)
        public void RepositionCards()
        {
            for (int i = 0; i < currentHand.Count; i++)
            {
                if (currentHand[i] == null) continue;
                if (i >= _cardSlots.Count) break; // 슬롯 개수 초과 방지

                Transform targetSlot = _cardSlots[i];

                currentHand[i].transform.DOKill();
                currentHand[i].transform.DOMove(targetSlot.position, cardAnimationDuration);
                currentHand[i].transform.DORotate(targetSlot.rotation.eulerAngles, cardAnimationDuration);
                // 혹시 스케일이 작아진 상태가 있다면 원래대로 복구
                currentHand[i].transform.DOScale(Vector3.one, cardAnimationDuration);
            }
        }
    }
}
