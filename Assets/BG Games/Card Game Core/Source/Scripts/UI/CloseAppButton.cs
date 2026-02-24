using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI
{
 
    [RequireComponent(typeof(Button))]
    public class CloseAppButton : MonoBehaviour
    {
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Close);
        }

        private void Close()
        {
            Application.Quit();
        }

    }
}