using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic.Basic
{
    public abstract class SpellLogicFactory:ScriptableObject
    {
        [Inject] protected IInstantiator Instantiator;

        public static List<IDescriptor> GetDefaultUntargetableDescriptors()
        {
            List<IDescriptor> descriptors = new List<IDescriptor>();
            descriptors.Add(new StealthDescriptor());

            return descriptors;
        }


        public abstract ISpellLogic CreateLogic(SpellCardInfo info);
        public abstract ITargetProvider CreateTargetProvider();
    }
}
