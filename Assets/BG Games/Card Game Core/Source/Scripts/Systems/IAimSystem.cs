using BG_Games.Card_Game_Core.Cards.Aiming;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    public interface IAimSystem
    {
        public IInputNeedyTargetProvider Requester { get; }

        public void StartAiming(IInputNeedyTargetProvider requester, Vector3 requesterPos);
        public void StartAiming(IInputNeedyTargetProvider requester);
        public void StopAiming();
    }
}
