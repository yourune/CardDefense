using TMPro;
using UnityEngine;

namespace BG_Games.Card_Game_Core.UI.InGame
{
    public enum MessageType
    {
        Player,
        Opponent
    }

    class BattleLogMessage:MonoBehaviour
    {
        [SerializeField] private RectTransform _blueBackground;
        [SerializeField] private RectTransform _redBackground;
        [SerializeField] private TMP_Text _messageText;

        public void SetMessage(string message, MessageType type)
        {
            _messageText.text = message;

            if (type == MessageType.Player)
            {
                _blueBackground.gameObject.SetActive(true);
                _redBackground.gameObject.SetActive(false);
            }
            else if (type == MessageType.Opponent)
            {
                _redBackground.gameObject.SetActive(true);
                _blueBackground.gameObject.SetActive(false);
            }
        }

        public void ResetMessage()
        {
            SetMessage("",MessageType.Player);
        }
    }

}
