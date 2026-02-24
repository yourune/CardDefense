using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Visual;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    public class AimSystem : MonoBehaviour,IAimSystem
    {
        [SerializeField] private AimVisual _visual;

        private bool _isAiming;
        private InputLockSystem _inputLock;

        public IInputNeedyTargetProvider Requester { get; private set; }

        [Inject]
        private void Construct(InputLockSystem inputLock)
        {
            _inputLock = inputLock;
        }

        private void Awake()
        {
            _visual.HideAim();
        }

        private void Update()
        {
            if (_isAiming)
            {
                _visual.AnimateAim(Input.mousePosition);

                if (Input.GetMouseButtonUp(1))
                {
                    CancelAim();
                }

                if (Input.GetMouseButtonUp(0))
                {
                   TryTakeAim(Input.mousePosition);
                }
            }
        }

        public void StartAiming(IInputNeedyTargetProvider requester, Vector3 requesterPos)
        {
            _visual.SetStartPosition(requesterPos);
            StartAiming(requester);
        }

        public void StartAiming(IInputNeedyTargetProvider requester)
        {
            Requester = requester;

            _inputLock.SetEnabledAllInput(false);
            _isAiming = true;
            _visual.ShowAim();
        }

        public void StopAiming()
        {
            _inputLock.SetEnabledAllInput(true);
            _isAiming = false;
           _visual.HideAim();
        }

        private void CancelAim()
        {
            Requester.AimCanceled();
        }

        private void TryTakeAim(Vector3 pointerScreenPosition)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerScreenPosition);

            if (Requester.TryFindTarget(worldPosition))
            {
                Requester.AimTaken();
            } 
        }
    }
}