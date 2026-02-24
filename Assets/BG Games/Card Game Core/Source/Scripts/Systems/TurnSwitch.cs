using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    public class TurnSwitch : MonoBehaviour
    {
        private PlayerRegistry _playerRegistry;
        private int _currentPlayer;

        [Inject]
        private void Construct(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        private void Start()
        {
            Player.Player firstPlayer = TossCoin();
            firstPlayer.ScheduleStart();

            foreach (var player in _playerRegistry.Players)
            {
                player.MatchStarted();
            }
        }

        public void TurnEnded()
        {
            _currentPlayer++;
            _currentPlayer %= _playerRegistry.Players.Length;

            _playerRegistry.Players[_currentPlayer].NextTurn();
        }

        private Player.Player TossCoin()
        {
            _currentPlayer = Random.Range(0,_playerRegistry.Players.Length);
            return _playerRegistry.Players[_currentPlayer];
        }
    }
}