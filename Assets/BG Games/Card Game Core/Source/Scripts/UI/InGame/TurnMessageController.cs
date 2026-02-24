using System;
using System.Collections;
using BG_Games.Card_Game_Core.Systems;
using TMPro;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.InGame
{
    public class TurnMessageController:MonoBehaviour
    {
        [SerializeField] private RectTransform _messagePanel;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TurnMessageConfig _turnMessageConfig;
        [Space] 
        [SerializeField] private float _disappearTime = 2;

        private PlayerRegistry _playerRegistry;

        [Inject]
        private void Construct(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        private void Awake()
        {
            _playerRegistry.LocalPlayer.OnTurnStart += PlayerTurn;
            _playerRegistry.GetOpponentOfPlayer(_playerRegistry.LocalPlayer.ID).OnTurnStart += EnemyTurn;
        }

        private void PlayerTurn() 
        {
            StopAllCoroutines();
            
            _messagePanel.gameObject.SetActive(true);
            _messageText.text = _turnMessageConfig.PlayerTurnMessage;
             
            StartCoroutine(WaitAndHideMessage());
        }

        private void EnemyTurn()
        {
            StopAllCoroutines();
            
            _messagePanel.gameObject.SetActive(true);
            _messageText.text = _turnMessageConfig.EnemyTurnMessage;

            StartCoroutine(WaitAndHideMessage());
        }

        private IEnumerator WaitAndHideMessage()
        {
            yield return new WaitForSeconds(_disappearTime);

            _messagePanel.gameObject.SetActive(false);
        }
    }
    
    [Serializable]
    public struct TurnMessageConfig
    {
        public string PlayerTurnMessage;
        public string EnemyTurnMessage;
    }
}
