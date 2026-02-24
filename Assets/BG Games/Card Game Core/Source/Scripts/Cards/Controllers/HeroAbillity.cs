using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public class HeroAbillity : MonoBehaviour,ISupportInputReading,ISupportLockInput,IActionLockable
    {
        [SerializeField] private HeroAbillityInfoHolder _infoHolder;

        private bool _isInputReadingLocked;
        private bool _actionsLocked;

        private IHeroLogic _heroLogic;
        private ITargetProvider _targetProvider;
        private PlayerId _owner;
        private Player.Player _player;

        private int _abillityCost;

        private InputLockSystem _inputLockSystem;
        private PlayerEnergy _playerEnergy;
        private PlayerHero _playerHero;

        [Inject]
        private void Construct(InputLockSystem inputLockSystem, PlayerEnergy energy, PlayerHero hero, Player.Player player)
        {
            _inputLockSystem = inputLockSystem;
            _inputLockSystem.AddInputReader(this);
            _playerEnergy = energy;
            _playerHero = hero;
            _player = player;
        }

        public void InitPosition(Transform positionPoint)
        {
            _infoHolder.transform.parent = positionPoint;
            _infoHolder.transform.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            _inputLockSystem.RemoveInputReader(this);
        }

        public void Init(IHeroLogic heroLogic, ITargetProvider targetProvider, PlayerId owner, HeroCardInfo info)
        {
            _heroLogic = heroLogic;
            _targetProvider = targetProvider;
            _owner = owner;
            _abillityCost = info.Cost;

            _infoHolder.Init(info);
        }

        public void SetEnabledInputReading(bool state)
        {
            _isInputReadingLocked = state;
        }

        public void MouseDown(Vector3 mousePosition, PlayerId player)
        {
            if (_isInputReadingLocked || player != _owner)
                return;

            if (!_actionsLocked)
            {
                if (_player.PlayCardInputSounds)
                    _playerHero.Card.Audio.ClickSound();
            }

        }

        public void MouseEnter(Vector3 mousePosition, PlayerId player)
        {
            _infoHolder.gameObject.SetActive(true);
        }

        public void MouseExit(Vector3 mousePosition, PlayerId player)
        {
            _infoHolder.gameObject.SetActive(false);
        }

        public void MouseUp(Vector3 mousePosition, PlayerId player)
        {
            if (_isInputReadingLocked || player != _owner)
                return;

            if (_actionsLocked != true && _heroLogic.AbillityAvailable && _playerEnergy.CanSpend(_abillityCost))
            {
                _targetProvider.OnAimEnd += EndAiming;

                TargetProvidersUtillity.StartAimWithPos(_targetProvider, _playerHero.Card.Position);
            }
            else
            {
                if (_player.PlayCardInputSounds)
                    _playerHero.Card.Audio.UnaccessSound();
            }
        }

        public void MouseDrag(Vector3 mousePosition, PlayerId player)
        {

        }

        private void EndAiming(bool targetTaken)
        {
            _targetProvider.OnAimEnd -= EndAiming;

            if (targetTaken)
            {
                _heroLogic.ApplyAbillity(_targetProvider);
                _playerEnergy.SpendEnergy(_abillityCost);
            }
        }

        public void SetLockActions(bool state)
        {
            _actionsLocked = state;
        }
    }
}
