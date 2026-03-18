using UnityEngine;
using UnityEngine.InputSystem;
using CardDefense.Core.Events;
using System;
using TMPro; // TextMeshPro 추가

namespace CardDefense.Cards // 네임스페이스 추가
{
    public class RefactoredCardView : MonoBehaviour
    {
        public string CardID { get; private set; }
        
        [SerializeField] private RefactoredCardData _cardData; // 인스펙터 테스트용
        public RefactoredCardData Data => _cardData;

        [Header("UI References")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _manaText;
        [SerializeField] private SpriteRenderer _imageSR;

        [Header("Settings")]
        [SerializeField] private LayerMask dropAreaLayer;

        private bool isDragging = false;
        private Vector3 startPosition;

        private void Start()
        {
            CardID = Guid.NewGuid().ToString();
        }

        // 스포너/덱에서 카드를 생성할 때 데이터 주입
        public void Setup(RefactoredCardData data)
        {
            _cardData = data;
            
            // 데이터 기반 UI 즉시 갱신
            if (_titleText != null) _titleText.text = data.Title;
            if (_descriptionText != null) _descriptionText.text = data.Description;
            if (_manaText != null) _manaText.text = data.ManaCost.ToString();
            if (_imageSR != null) _imageSR.sprite = data.Image;
        }

        private void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            Vector2 mousePos2D = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos2D);
            bool isHovering = Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == this.gameObject;

            if (Mouse.current.leftButton.wasPressedThisFrame && isHovering)
            {
                isDragging = true;
                startPosition = transform.position;
            }

            if (isDragging)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, Camera.main.nearClipPlane + 5f));
                    transform.position = p;
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    isDragging = false;
                    CheckDropAndPlay(mousePos2D);
                }
            }
        }

        private void CheckDropAndPlay(Vector2 screenMousePos)
        {
            if (_cardData == null) 
            {
                Debug.LogError("카드 데이터가 세팅되지 않았습니다!");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(screenMousePos);
            // DropArea(땅)을 맞췄는지 확인
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, dropAreaLayer))
            {
                PlayCard(hit.point);
            }
            else
            {
                transform.position = startPosition;
                FindObjectOfType<RefactoredHandSystem>()?.RepositionCards();
            }
        }

        private void PlayCard(Vector3 dropPosition)
        {
            // EventBus에 '나 무슨 카드(Data) 냈음' 이라고 쏴줍니다.
            // 데이터 그 자체(CardData)를 포함해서 쏘는 구조체(Event)를 만들 것입니다.
            EventBus.Publish(new CardUsedEvent
            {
                CardID = this.CardID,
                CastPosition = dropPosition,
                PlayedCard = this.Data // 사용된 카드의 스펙을 함께 넘김
            });
        }
    }
}
