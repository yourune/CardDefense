using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    [CreateAssetMenu(fileName = "SpellLogic", menuName = "ScriptableObjects/Spell Logic/Damage Enemy")]
    class DamageEnemyFactory:SpellLogicFactory
    {
        [SerializeField] private int _damage = 3;

        public override ISpellLogic CreateLogic(SpellCardInfo info)
        {
            DamageEnemy logic = Instantiator.Instantiate<DamageEnemy>(new object[] {info, _damage });
            return logic;
        }

        public override ITargetProvider CreateTargetProvider()
        {
            SingleEnemyProvider targetProvider = Instantiator.Instantiate<SingleEnemyProvider>();
            targetProvider.ForbiddenDescriptors = GetDefaultUntargetableDescriptors();
            return targetProvider;
        }
    }
}
