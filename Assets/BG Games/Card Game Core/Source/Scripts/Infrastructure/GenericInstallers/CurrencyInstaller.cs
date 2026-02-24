using BG_Games.Card_Game_Core.Systems.CurrencySystem;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    public class CurrencyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICurrencyService>().To<CurrencyService>().AsSingle();
        }
    }
}
