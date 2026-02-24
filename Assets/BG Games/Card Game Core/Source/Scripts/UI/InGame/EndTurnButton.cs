using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.InGame
{
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField] private PlayerId _playerID;
        [Space]
        [SerializeField] private Button _button;
        [Header("Disabled")]
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Color _disabledColor;

        private Color _savedBackColor;
        private Color _savedTextColor;

        private Player.Player _player;

        [Inject]
        private void Construct(PlayerRegistry playerRegistry,TurnSwitch turnSwitch)
        {
            _player = playerRegistry.GetPlayerByID(_playerID);
        }

        private void Awake()
        {
            _button.onClick.AddListener(Click);

            _player.OnTurnEnd += TurnEnd;
            _player.OnTurnStart += TurnStart;

            TurnEnd();
        }

        private void TurnEnd()
        {
            _savedBackColor = _background.color;
            _savedTextColor = _text.color;

            _background.color = _disabledColor;
            _text.color = _disabledColor;
            _button.interactable = false;
        }

        private void TurnStart()
        {
            _background.color = _savedBackColor;
            _text.color = _savedTextColor;
            _button.interactable = true;
        }

        private void Click()
        {
            _player.EndTurn();
        }
    }
}