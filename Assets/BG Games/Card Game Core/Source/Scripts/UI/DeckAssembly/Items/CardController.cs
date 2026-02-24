using System;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Items
{
    public class CardController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
    {
        [field: SerializeField] public CardInfoHolder CardInfoHolder { get; private set; }

        private int _defaultIndex;

        public CardInfo Info { get; protected set; }

        public event Action<CardController> OnDragStarted;
        public event Action<CardController> OnDragEnded;

        public event Action<CardController> OnClick;

        public bool Draggable { get; set; }

        public void InitInfo(CardInfo info)
        {
            Info = info;
            CardInfoHolder.InitInfo(info);
            CardInfoHolder.SetMode(CardViewMode.Front);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Draggable)
            {
                OnDragStarted?.Invoke(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Draggable)
            {
                OnDragEnded?.Invoke(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Draggable)
            {
                transform.position = Input.mousePosition;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }

    }
}
