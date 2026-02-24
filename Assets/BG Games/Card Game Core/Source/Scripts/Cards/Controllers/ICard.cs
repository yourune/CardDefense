using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public interface ICard
    {
        CardInfo Info { get; }
        Vector3 Position { get; }
    }
}
