using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    public class UnitAnimController:CardAnimController
    {
        [field:SerializeField] public HealthAnimations HealthAnimations { get; private set; }
        [field:SerializeField] public AttackAnimations AttackAnimations { get; private set; }
        [field:SerializeField] public DeathAnimations DeathAnimations { get; private set; }



        public void Init(IUnitLogic cardLogic, UnitInfoHolder infoHolder)
        {
            HealthAnimations.Init(cardLogic, infoHolder);
            AttackAnimations.Init(cardLogic);
            DeathAnimations.Init(cardLogic);
        }


    }
}
