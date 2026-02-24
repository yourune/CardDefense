using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers.Animations;
using BG_Games.Card_Game_Core.Cards.Controllers.Audio;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public class Hero : MonoBehaviour,ITroopCard,ICard,ISupportLockInput,ISupportInputReading,IActionLockable
    {
        [SerializeField] protected HeroInfoHolder InfoHolder;
        [SerializeField] private HeroAbillity _abillity;
        [SerializeField] private HeroAnimController _animController;
        [SerializeField] private HeroAudio _audio;

        public CardInfo Info => InfoHolder.Info;
        public HeroCardInfo HeroInfo { get; private set; }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public ITroopLogic Logic => HeroLogic;
        public IHeroLogic HeroLogic { get; private set; }
        public ITargetProvider TargetProvider { get; private set; }

        public HeroAudio Audio => _audio;
        public HeroAbillity Abillity => _abillity;

        private InputLockSystem _inputLockSystem;
        private PlayerId _owner;

        private bool _actionsLocked = false;
        private bool _isInputReadingLocked;

        [Inject]
        private void Construct(InputLockSystem inputLockSystem, PlayerId owner)
        {
            _owner = owner;
            _inputLockSystem = inputLockSystem;
            _inputLockSystem.AddInputReader(this);
        }

        public void InitPosition(Transform positionPoint)
        {
            transform.parent = positionPoint;
            transform.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            _inputLockSystem.RemoveInputReader(this);
        }

        public void NextTurn()
        {
            HeroLogic.NextTurn();
        }

        public void EndTurn()
        {
            HeroLogic.EndTurn();
        }

        public void InitInfo(HeroCardInfo info)
        {
            HeroLogic = info.GetCardLogic();
            TargetProvider = info.GetTargetProvider();

            InfoHolder.InitInfo(info);
            HeroInfo = info;

            _abillity.Init(HeroLogic, TargetProvider, _owner, info);

            _animController.Init(Position, HeroLogic, InfoHolder);

            _audio.Init(this);
        }

        public void SetEnabledInputReading(bool state)
        {
            _isInputReadingLocked = !state;
            _abillity.SetEnabledInputReading(!state);
        }


        public void MouseDown(Vector3 mousePosition, PlayerId player)
        {

        }

        public void MouseUp(Vector3 mousePosition, PlayerId player)
        {

        }

        public void MouseDrag(Vector3 mousePosition, PlayerId player)
        {

        }

        public void MouseEnter(Vector3 mousePosition, PlayerId player)
        {

        }
        public void MouseExit(Vector3 mousePosition, PlayerId player)
        {

        }

        public void SetLockActions(bool state)
        {
            _actionsLocked = state;
            _abillity.SetLockActions(state);
        }
    }
}
