using System;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    class MatchEnd:MonoBehaviour
    {
        private Player.Player _localPlayer;
        private Player.Player _opponentPlayer;

        public event Action OnWin;
        public event Action OnDefeat;
        public event Action OnDraw;
        
        [Inject]
        private void Construct(PlayerRegistry playerRegistry)
        {
            _localPlayer = playerRegistry.LocalPlayer;
            _opponentPlayer = playerRegistry.GetOpponentOfPlayer(playerRegistry.LocalPlayer.ID);
        }

        private void Awake()
        {
            _localPlayer.Hero.OnHeroDead += LocalLose;
            _opponentPlayer.Hero.OnHeroDead += LocalWin;
            _localPlayer.Hero.OnHeroDraw += LocalDraw;
        }

        private void LocalDraw()
        {
            NotifyPlayers();
            
            OnDraw?.Invoke();
        }
        
        private void LocalWin()
        {
            NotifyPlayers();

            OnWin?.Invoke();
        }

        private void LocalLose()
        {
            NotifyPlayers();

            OnDefeat?.Invoke();
        }

        private void NotifyPlayers()
        {
            _localPlayer.MatchEnd();
            _opponentPlayer.MatchEnd();
        }
    }
}
