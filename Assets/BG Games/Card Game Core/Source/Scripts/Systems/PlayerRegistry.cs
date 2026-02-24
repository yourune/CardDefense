using System.Linq;
using BG_Games.Card_Game_Core.Player;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    public class PlayerRegistry:MonoBehaviour
    {
        [field: SerializeField] public PlayerId LocalPlayerID { get; private set; } 
        [field: SerializeField] public Player.Player[] Players { get; private set; }

        private Player.Player localPlayer;

        public Player.Player LocalPlayer
        {
            get
            {
                if (localPlayer == null)
                {
                    localPlayer = Players.First(player => player.ID == LocalPlayerID);
                }

                return localPlayer;
            }
        }

        public Player.Player GetOpponentOfPlayer(PlayerId playerId)
        {
            return Players.First(player => player.ID != playerId);
        }

        public Player.Player GetPlayerByID(PlayerId id)
        {
            return Players.First(player => player.ID == id);
        }
    }
}
