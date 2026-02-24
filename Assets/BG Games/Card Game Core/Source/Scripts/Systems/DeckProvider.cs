using BG_Games.Card_Game_Core.Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    public abstract class DeckProvider:MonoBehaviour
    {
        public abstract UniTask<DeckData> GetDeck();
    }
}
