using System;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    public enum TargetType
    {
        Enemy,
        Ally
    }

    public interface IInputNeedyTargetProvider:ITargetProvider
    {
        public TargetType TargetType { get; }

        public event Action OnAimStart;

        public void StartAim(Vector3 pos);
        public bool TryFindTarget(Vector3 targetPos);
        public void AimCanceled();
        public void AimTaken();
    }
}
