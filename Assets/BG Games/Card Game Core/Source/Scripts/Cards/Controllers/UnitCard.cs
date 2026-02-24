using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers.Animations;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public class UnitCard : Card,ITroopCard,ITurnCallbacksListener
    {
        [SerializeField] private UnitInfoHolder _infoHolder;
        [Space]
        [SerializeField] protected UnitAnimController _animController;

        public event Action OnPlaced;

        protected GameTable Table;

        public override bool ShouldRemainOnTable => true;

        protected override DragAnimations DragAnimations => _animController.DragAnimations;
        protected override CardInfoHolder InfoHolder => _infoHolder;
        public override CardInfo Info => _infoHolder.Info;
        public UnitCardInfo UnitInfo { get; protected set; }

        public ITargetProvider TargetProvider { get; protected set; }
        public IUnitLogic UnitLogic { get; private set; }
        public ITroopLogic Logic => UnitLogic;


        [Inject]
        private void Construct(GameTable table)
        {
            Table = table;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveCardFromTable();
        }

        protected override void Start()
        {
            InfoHolder.SetMode(Player.CardViewMode);
        }

        public void InitInfo(UnitCardInfo info)
        {
            _infoHolder.InitInfo(info);
            UnitInfo = info;

            Cost = info.Cost;

            TargetProvider = info.GetTargetProvider();
            UnitLogic = info.GetCardLogic();
            UnitLogic.SubscribeCardControllerCallbacks(this);

            UnitLogic.OnDPChanged += dp => _infoHolder.ChangeDP(dp);
            UnitLogic.OnDead += Dead;

            _animController.Init(UnitLogic,_infoHolder);
            CardAudio.Init(this);
        }


        public void NextTurn()
        {
            UnitLogic.NextTurn();
        }

        public void EndTurn()
        {
            UnitLogic.EndTurn();
        }

        public override void Placed()
        {
            base.Placed();
            Table.GetMyTableSide(Owner).PlaceCard(this);
            OnPlaced?.Invoke();
        }

        public virtual void Returned()
        {
            IsPlaced = false;
            UnitLogic.ResetCard();

            InfoHolder.SetMode(Player.CardViewMode);
        }

        public void RemoveCardFromTable()
        {
            if (IsPlaced)
            {
                Table.GetMyTableSide(Owner).RemoveCard(this);
                IsPlaced = false;
            }
        }

        public void RemoveCardFromTable(float timeDelay)
        {
            if (IsPlaced)
            {
                Table.GetMyTableSide(Owner).RemoveCardDelayVisual(this,timeDelay);
                IsPlaced = false;
            }
        }

        public override void MouseUp(Vector3 mousePosition,PlayerId player)
        {
            if (IsInputReadingLocked || player != Owner)
                return;

            if (IsPlaced)
            {
                if (UnitLogic.ActionAvailable && AskCanBePlayed())
                {
                    TargetProvider.OnAimEnd += EndAiming;
                    TargetProvidersUtillity.StartAimWithPos(TargetProvider, Position);
                }
                else
                {
                    if (Player.PlayCardInputSounds)
                        CardAudio.UnaccessSound();
                }
            }
            else
            {
                base.MouseUp(mousePosition,player);
            }

        }

        private void EndAiming(bool targeted)
        {
            TargetProvider.OnAimEnd -= EndAiming;

            if (targeted)
            {
                UnitLogic.DoMainAction(TargetProvider);
            }
        }

        private void Dead()
        {
            RemoveCardFromTable(_animController.DeathAnimations.AnimationsDuration);
            InputLockSystem.RemoveInputReader(this);
            Collider.enabled = false;
        }

    }
}