using BG_Games.Card_Game_Core.Systems;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    public class DeckSaveInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDeckSaveSystem>().To<DeckLocalSave>().AsSingle();
        }
    }
}