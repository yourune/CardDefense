using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers.Animations;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public class SpellCard:Card
    {
        [SerializeField] private Canvas _infoScreen;
        [SerializeField] private CardInfoHolder _infoHolder;
        [SerializeField] private SpellAnimController _animController;

        protected override DragAnimations DragAnimations => _animController.DragAnimations;
        public override bool ShouldRemainOnTable => false;
        protected override CardInfoHolder InfoHolder => _infoHolder;
        public override CardInfo Info => _infoHolder.Info;
        public SpellCardInfo SpellInfo { get; protected set; }

        public ITargetProvider TargetProvider { get; protected set; }
        public ISpellLogic SpellLogic { get; private set; }

        private PlayerHero hero;
        private PlayerHand hand;
        private PlayerEnergy energy;

        [Inject]
        private void Construct(PlayerHero playerHero, PlayerHand playerHand, PlayerEnergy playerEnergy)
        {
            hero = playerHero;
            hand = playerHand;
            energy = playerEnergy;
        }

        protected override void Start()
        {
            InfoHolder.SetMode(Player.CardViewMode);
        }

        public void InitInfo(SpellCardInfo info)
        {
            _infoHolder.InitInfo(info);
            SpellInfo = info;

            Cost = info.Cost;

            TargetProvider = info.GetTargetProvider();
            SpellLogic = info.GetLogic();

            CardAudio.Init(this);
        }

        public override void Placed()
        {
            TargetProvider.OnAimEnd += EndAiming;
            TargetProvidersUtillity.StartAimWithPos(TargetProvider,hero.Card.Position);

            CardAudio.PlaceSound();

            HideView();
        }

        private void EndAiming(bool targeted)
        {
            TargetProvider.OnAimEnd -= EndAiming;

            if (targeted)
            {
                IsPlaced = true;
                SetToDefaultLayer();
                InfoHolder.SetMode(CardViewMode.Front);
                SpellLogic.CastSpell(TargetProvider);
                
                Destroy(gameObject);
            }
            else
            {
                hand.AddCard(this);
                energy.AddExtraEnergy(Cost);
                IsPlaced = false;
                ShowView();
            }
        }

        private void HideView()
        {
            _infoScreen.enabled = false;
        }

        private void ShowView()
        {
            _infoScreen.enabled = true;
        }
    }
}
