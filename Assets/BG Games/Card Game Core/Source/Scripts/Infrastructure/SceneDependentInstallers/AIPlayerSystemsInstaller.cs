using BG_Games.Card_Game_Core.AI;
using BG_Games.Card_Game_Core.AI.TurnOrder;
using BG_Games.Card_Game_Core.Player.TurnOrder;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Infrastructure.SceneDependentInstallers
{
    class AIPlayerSystemsInstaller:PlayerSystemsInstaller
    {
        [SerializeField] private InputEmulator _inputEmulator;
        [SerializeField] private TestBot _bot;

        public override void InstallBindings()
        {
            base.InstallBindings();

            InstallInputEmulator();
            InstallBot();
        }

        protected override void InstallAimSystem()
        {
            AimSystemAI aimSystem = Container.Instantiate<AimSystemAI>();

            Container.BindInterfacesAndSelfTo<AimSystemAI>().FromInstance(aimSystem).AsSingle();
        }

        private void InstallBot()
        {
            Container.Bind<TestBot>().FromInstance(_bot).AsSingle();
        }

        private void InstallInputEmulator()
        {
            Container.Bind<InputEmulator>().FromInstance(_inputEmulator).AsTransient();
        }

        protected override void InstallTurnOrderStates()
        {
            Container.Bind<StartMatchTurnState>().FromNew().AsTransient();
            Container.Bind<FirstPreTurnState>().FromNew().AsTransient();
            Container.Bind<PreTurnState>().FromNew().AsTransient();
            Container.Bind<MainTurnState>().To<AIMainTurnState>().FromNew().AsTransient();
            Container.Bind<WaitingTurnState>().FromNew().AsTransient();
            Container.Bind<MulliganTurnState>().FromNew().AsTransient();
            Container.Bind<MatchEndTurnState>().FromNew().AsTransient();
        }
    }
}
