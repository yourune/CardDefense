using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI
{
    public class Postman : MonoBehaviour
    {
        [SerializeField] private RectTransform _screen;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        public void Open(string message)
        {
            _screen.gameObject.SetActive(true);
            _messageText.text = message;
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
        }
    }
}