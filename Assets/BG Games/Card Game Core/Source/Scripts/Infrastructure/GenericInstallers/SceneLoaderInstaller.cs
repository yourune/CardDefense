using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoader _sceneLoader;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromComponentInNewPrefab(_sceneLoader).AsSingle();
        }
    }
}