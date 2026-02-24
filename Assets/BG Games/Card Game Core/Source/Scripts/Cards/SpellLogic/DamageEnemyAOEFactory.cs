using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    [CreateAssetMenu(fileName = "SpellLogic", menuName = "ScriptableObjects/Spell Logic/Damage AOE")]
    class DamageEnemyAOEFactory:SpellLogicFactory
    {
        [SerializeField] private int _primaryDamage;
        [SerializeField] private int _adjacentDamage;

        public override ISpellLogic CreateLogic(SpellCardInfo info)
        {
            DamageEnemyAOE logic = Instantiator.Instantiate<DamageEnemyAOE>(new object[]{info, _primaryDamage, _adjacentDamage} );
            return logic;
        }

        public override ITargetProvider CreateTargetProvider()
        {
            EnemyAndAdjacentProvider targetProvider = Instantiator.Instantiate<EnemyAndAdjacentProvider>();
            targetProvider.ForbiddenDescriptors = GetDefaultUntargetableDescriptors();
            return targetProvider;
        }
    }
}
