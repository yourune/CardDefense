using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Tools;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Visual
{
    enum ScaleMode
    {
        Delta,
        Absolute
    }

    class CardsScaledPlacementGroup:CardsPlacementGroup
    {
        [SerializeField] private ScaleMode _mode = ScaleMode.Delta;
        [SerializeField] private Vector3 _scale = Vector3.one;

        private Vector3 _savedScale;

        public override void AddCard(Card card)
        {
            base.AddCard(card);
            Scale(card);
        }

        public override void RemoveCard(Card card)
        {
            Unscale(card);
            base.RemoveCard(card);
        }

        private void Scale(Card card)
        {
            if (_mode == ScaleMode.Delta)
            {
                _savedScale = card.transform.localScale;
                card.transform.localScale = card.transform.localScale.MultiplyByCoordinates(_scale);
            }
            else if (_mode == ScaleMode.Absolute)
            {
                _savedScale = card.transform.localScale;
                card.transform.localScale = _scale;
            }
        }

        private void Unscale(Card card)
        {
            if (_mode == ScaleMode.Delta)
            {
                card.transform.localScale = _savedScale;
            }
            else if (_mode == ScaleMode.Absolute)
            {
                card.transform.localScale = _savedScale;
            }
        }
    }
}
