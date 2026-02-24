using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;

namespace BG_Games.Card_Game_Core.AI
{
    class AimSystemAI:IAimSystem
    {
        private Vector3 _presetTarget;
        private bool _specifiedTarget;

        public IInputNeedyTargetProvider Requester { get; private set; }
        
        public event Func<IInputNeedyTargetProvider, Vector3> OnNotSpecifiedTarget;

        public void StartAiming(IInputNeedyTargetProvider requester, Vector3 requesterPos)
        {
            StartAiming(requester);
        }

        public void StartAiming(IInputNeedyTargetProvider requester)
        {
            Requester = requester;
            if (_specifiedTarget && Requester.TryFindTarget(_presetTarget))
            {
                Requester.AimTaken();
            }
            else
            {
                Vector3 target = OnNotSpecifiedTarget.Invoke(Requester);

                if (Requester.TryFindTarget(target))
                {
                    Requester.AimTaken();
                }
                else
                {
                    Requester.AimCanceled();
                }
            }
        }

        public void StopAiming()
        {
            _specifiedTarget = false;
            _presetTarget = Vector3.zero;
            Requester = null;
        }        
        
        public void PreSetTarget(Vector3 target)
        {
            _presetTarget = target;
            _specifiedTarget = true;
        }
    }
}
