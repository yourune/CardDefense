using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic.Basic
{
    public abstract class UnitLogicFactory:ScriptableObject
    {
        [Inject] protected IInstantiator Instantiator;

        public abstract IUnitLogic CreateLogic(UnitCardInfo info);

        public virtual ITargetProvider CreateTargetProvider()
        {
            SingleEnemyProvider provider = Instantiator.Instantiate<SingleEnemyProvider>();
            provider.ForbiddenDescriptors = GetDefaulUntargetableDescriptors();
            return provider;
        }

        public static List<IDescriptor> GetDefaulUntargetableDescriptors()
        {
            List<IDescriptor> descriptors = new List<IDescriptor>();
            descriptors.Add(new TauntedDescriptor());
            descriptors.Add(new StealthDescriptor());

            return descriptors;
        }
    }
}
