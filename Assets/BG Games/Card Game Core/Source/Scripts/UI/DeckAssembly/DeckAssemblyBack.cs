using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    public class DeckAssemblyBack : MonoBehaviour
    {
        [SerializeField] private Button _backButton;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _backButton.onClick.AddListener(ToMainMenu);
        }

        private void ToMainMenu()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}