using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic.Basic
{
    public abstract class HeroLogicFactory : ScriptableObject
    {
        [Inject] protected IInstantiator Instantiator;

        public abstract IHeroLogic CreateLogic(HeroCardInfo info);
        public abstract ITargetProvider CreateTargetProvider();

        protected List<IDescriptor> GetDefaultForbiddenDescriptors()
        {
            List<IDescriptor> descriptors = new List<IDescriptor>();
            descriptors.Add(new StealthDescriptor());

            return descriptors;
        }
    }
}
