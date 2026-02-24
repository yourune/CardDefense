using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Items
{
    public interface ICardInfoHolder
    {
        public CardInfo Info { get; }
        public Transform Transform { get; }
    }
}
