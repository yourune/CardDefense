using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI
{
    class TopLayerHaulerInstaller:MonoInstaller
    {
        [SerializeField] private CanvasTopLayerHauler _topLayerHauler;

        public override void InstallBindings()
        {
            Container.Bind<ITopLayerHauler>().FromInstance(_topLayerHauler);
        }
    }
}
