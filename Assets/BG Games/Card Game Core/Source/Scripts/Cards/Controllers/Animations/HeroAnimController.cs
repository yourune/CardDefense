using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    class HeroAnimController:MonoBehaviour
    {
        [field:SerializeField] public HealthAnimations HealthAnimations { get; private set; }
        [field:SerializeField] public DeathAnimations DeathAnimations { get; private set; }

        public void Init(Vector3 origin,IHeroLogic logic, HeroInfoHolder infoHolder)
        {
            HealthAnimations.Init(logic,infoHolder);
            DeathAnimations.Init(logic);
        }
    } 
}
