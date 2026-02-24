using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Visual
{
    class CardsCurvePlacementGroup:CardsPlacementGroup
    {
        [SerializeField] protected float CurveRadius;
        [SerializeField] protected Vector3 CenterOffset = Vector3.down;

        protected Vector2 CurveCenter => transform.position + CenterOffset.normalized * CurveRadius;

        public override void RepaintCards()
        {
            float totalWidth = CalculateContentWidth();

            if (totalWidth == 0)
                return;

            float previousCardZ = 0;

            Vector3 cardPosition = FindStartPoint();
            cardPosition.z = transform.position.z;


            foreach (Card card in Content)
            {

                card.Position = cardPosition;
                card.transform.rotation = Quaternion.Euler(new Vector3(0,0, FindCardRotation(cardPosition)));
                cardPosition = FindPointOnCircle(cardPosition, Offset + CardWidth, true);

                if (ZInOrder)
                {
                    previousCardZ -= ZOffset;
                    cardPosition.z = previousCardZ;
                }
            }
        }

        private Vector3 FindStartPoint()
        {
            Vector3 startPosition;

            if (Content.Count % 2 > 0)
            {
                float startOffset = (Offset + CardWidth) * (int)(Content.Count / 2);

                startPosition = FindPointOnCircle(transform.position, startOffset, false);
                startPosition.z = transform.position.z;
            }
            else
            {
                float startOffset = ((Offset + CardWidth) / 2) + (Offset + CardWidth) * ((int)(Content.Count / 2) - 1);

                startPosition = FindPointOnCircle(transform.position, startOffset, false);
                startPosition.z = transform.position.z;
            }

            return startPosition;
        }

        private float FindCardRotation(Vector3 cardPosition)
        {
            Vector2 cardNormal = CurveCenter - (Vector2)cardPosition;
            Vector2 cardDirection = Vector2.up - cardNormal;

            float deltaAngle = Mathf.Atan2(cardDirection.x, cardDirection.y) * Mathf.Rad2Deg;

            return 360 - deltaAngle;
        }

        public Vector2 FindPointOnCircle(Vector2 start, float arcLength, bool clockwise = false)
        {
            float angleRadians = arcLength / CurveRadius;

            float startAngle = Mathf.Atan2(start.y - CurveCenter.y, start.x - CurveCenter.x);

            float newAngle = clockwise ? startAngle - angleRadians : startAngle + angleRadians;

            float newX = CurveCenter.x + CurveRadius * Mathf.Cos(newAngle);
            float newY = CurveCenter.y + CurveRadius * Mathf.Sin(newAngle);

            return new Vector2(newX, newY);
        }
    }
}
