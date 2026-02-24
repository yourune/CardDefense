using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.SaveSystem;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    public class SaversInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerInventorySaveSystem>().To<PlayerInventorySaveSystem>().AsSingle();
        }
    }
}
