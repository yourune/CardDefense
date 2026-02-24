using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.InGame
{
    class BattleLogController:MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _logPanel;
        [SerializeField] private BattleLogMessage[] _messages;

        private int _messageIndex = 0;

        private void Awake()
        {
            _openButton.onClick.AddListener(OpenLog);
            _closeButton.onClick.AddListener(CloseLog);
        }

        private void OpenLog()
        {
            _logPanel.gameObject.SetActive(true);
            _openButton.gameObject.SetActive(false);
        }

        private void CloseLog()
        {
            _logPanel.gameObject.SetActive(false);
            _openButton.gameObject.SetActive(true);
        }

        [Inject]
        private void Construct(BattleLog log)
        {
            log.OnAddedMessage += OnAddMessage;
        }

        private void OnAddMessage(string message, PlayerId owner)
        {
            MessageType type = owner == PlayerId.Player1 ? MessageType.Player : MessageType.Opponent;

            int fullness = _messageIndex / _messages.Length;
            int scaledIndex = _messageIndex % _messages.Length;

            _messageIndex++;

            _messages[scaledIndex].SetMessage(message, type);

            if (fullness > 0)
            {
                _messages[scaledIndex].transform.SetAsLastSibling();
            }

        }
    }
}
