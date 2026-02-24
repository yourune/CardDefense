using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    class BattlecryHealUnit:UnitCardLogic,ILogicWithSFX
    {
        public event Action OnSFXApply;

        private int _healAmount;
        private SingleAllyProvider _abillityTargetProvider;
        private Transform _cardTransform;

        public BattlecryHealUnit(UnitCardInfo info,int healAmount, SingleAllyProvider abillityTargetProvider) : base(info)
        {
            _abillityTargetProvider = abillityTargetProvider;
            _healAmount = healAmount;
        }

        public override void SubscribeCardControllerCallbacks(UnitCard controller)
        {
            _cardTransform = controller.transform;
            controller.OnPlaced += ApplyAbillity;
        }

        private void ApplyAbillity()
        {
            _abillityTargetProvider.OnAimEnd += AimEnd;
            
            TargetProvidersUtillity.StartAimWithPos(_abillityTargetProvider,_cardTransform.position);
        }

        private void AimEnd(bool targeted)
        {
            _abillityTargetProvider.OnAimEnd -= AimEnd;

            if (targeted)
            {
                _abillityTargetProvider.Target.Logic.RestoreHP(_healAmount);
                OnSFXApply?.Invoke();
            }
        }
    }
}
