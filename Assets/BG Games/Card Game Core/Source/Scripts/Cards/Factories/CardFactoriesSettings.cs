using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Factories
{
    class CardFactoriesSettings:MonoBehaviour
    {
        [field: SerializeField] public UnitCard UnitPrefab { get; private set; }
        [field: SerializeField] public SpellCard SpellPrefab { get; private set; }
        [field: SerializeField] public Hero HeroPrefab { get; private set; }
    }
}
