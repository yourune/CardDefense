using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using TMPro;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.InGame
{
    class EnergyDisplay:MonoBehaviour
    {
        [SerializeField] private TMP_Text _energyValue;
        [SerializeField] private TMP_Text _energyTotalValue;
        [SerializeField] private PlayerId _playerID;

        private Player.Player _player;

        private const string TotalSplitter = "/";

        [Inject]
        private void Construct(PlayerRegistry playerRegistry)
        {
            _player = playerRegistry.GetPlayerByID(_playerID);
        }

        private void Awake()
        {
            _player.Energy.OnEnergyBalanceChanged += EnergyBalanceChanged;
        }

        private void EnergyBalanceChanged(int value)
        {
            _energyValue.text = value.ToString();
            _energyTotalValue.text = TotalSplitter + _player.Energy.TurnEnergy.ToString();
        }
    }
}
