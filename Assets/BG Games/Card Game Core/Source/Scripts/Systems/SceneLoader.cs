using Template.H_lib.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BG_Games.Card_Game_Core.Systems
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string _mainMenuScene;
        [SerializeField] private string _gameScene;
        [SerializeField] private string _deckBuildingScene;

        public void LoadGame()
        {
            SceneManager.LoadSceneAsync(_gameScene);
        }

        public void LoadDeckBuilding()
        {
            SceneManager.LoadSceneAsync(_deckBuildingScene);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync(_mainMenuScene);
        }

    }
}