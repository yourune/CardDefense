using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Visual
{
    public class CardsPlacementGroup : MonoBehaviour
    {
        [field: SerializeField] public List<Card> Content { get; private set; }
        [Space] 
        [SerializeField] protected bool ZInOrder = false;
        [SerializeField] protected float ZOffset = 0.01f;
        [SerializeField] protected float Offset;
        [SerializeField] protected float CardWidth;

        public virtual void InsertCard(Card card, out int position)
        {
            position = FindPositionToInsert(card);
            Content.Insert(position, card);
            RepaintCards();
        }

        public virtual void AddCard(Card card)
        {
            Content.Add(card);
            RepaintCards();
        }

        public virtual void RemoveCard(Card card)
        {
            Content.Remove(card);
            RepaintCards();
        }

        public virtual void RepaintCards()
        {
            float totalWidth = CalculateContentWidth();

            if (totalWidth == 0 || this == null)
                return;

            float startX = transform.position.x - (totalWidth / 2) + (CardWidth / 2);
            float startY = transform.position.y;
            float startZ = transform.position.z;

            Vector3 cardPosition = new Vector3(startX, startY, startZ);
        
            foreach(Card card in Content)
            {
                card.Position = cardPosition;
                cardPosition.x += CardWidth + Offset;

                if (ZInOrder)
                {
                    cardPosition.z -= ZOffset;
                }
            }
        }

        protected virtual int FindPositionToInsert(Card card)
        {
            if (Content.Count == 0)
                return 0;

            int position = 0;
            bool found = false;

            for(int i = 0; i < Content.Count; i++)
            {
                if (Content[i].Position.x > card.Position.x)
                {
                    position = i;
                    found = true;
                    break;
                }
            }

            return found ? position : Content.Count;
        }

        protected virtual float CalculateContentWidth()
        {
            if (Content == null || Content.Count == 0)
                return 0;

            float cardsWidth = Content.Count * CardWidth;
            float cardsOffsets = (Content.Count - 1) * Offset;

            return cardsWidth + cardsOffsets;
        }
    }
}
