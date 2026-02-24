using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    public abstract class ConcreteTargetProvider:IInputNeedyTargetProvider
    {
        public event Action<bool> OnAimEnd;
        public event Action OnAimStart;

        public List<IDescriptor> ForbiddenDescriptors { get; set; }

        public abstract TargetType TargetType { get; }

        protected Vector3 StartPos;
        protected IAimSystem AimSystem;
        protected int CardsLayerMask;
        protected GameTable GameTable;

        public ConcreteTargetProvider(PlayerId owner, IAimSystem aimSystem, GameTable table, [Inject(Id = "Attackable")] LayerMask cardsLayerMask,List<IDescriptor> forbiddenDescriptors)
        {
            AimSystem = aimSystem;
            GameTable = table;
            CardsLayerMask = cardsLayerMask;
            ForbiddenDescriptors = forbiddenDescriptors;
        }

        public ConcreteTargetProvider(PlayerId owner, IAimSystem aimSystem, GameTable table, [Inject(Id = "Attackable")] LayerMask cardsLayerMask)
        {
            AimSystem = aimSystem;
            GameTable = table;
            CardsLayerMask = cardsLayerMask;
            ForbiddenDescriptors = new List<IDescriptor>();
        }

        public virtual bool TryFindTarget(Vector3 worldPointerPosition)
        {
            worldPointerPosition.z = 0;

            if (ValidatePoint(worldPointerPosition))
            {
                RaycastHit2D hit = Physics2D.Raycast(worldPointerPosition, Vector2.zero, Mathf.Infinity, CardsLayerMask);

                if (hit.collider != null)
                {
                    return TrySetTarget(hit.collider);
                }
            }

            return false;
        }

        public void StartAim(Vector3 pos)
        {
            StartPos = pos;
            AimSystem.StartAiming(this,StartPos);
            OnAimStart?.Invoke();
        }

        public void StartAim()
        {
            AimSystem.StartAiming(this);
            OnAimStart?.Invoke();
        }

        public void AimCanceled()
        {
            AimSystem.StopAiming();
            OnAimEnd?.Invoke(false);
        }

        public void AimTaken()
        {
            AimSystem.StopAiming();
            OnAimEnd?.Invoke(true);
        }
        
        protected abstract bool ValidatePoint(Vector3 point);

        protected abstract bool TrySetTarget(Collider2D raycastResult);
    }
}
