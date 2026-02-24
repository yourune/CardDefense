using BG_Games.Card_Game_Core.Player;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    interface ISupportInputReading
    {
        public void MouseDown(Vector3 mousePosition, PlayerId player);
        public void MouseUp(Vector3 mousePosition, PlayerId player);
        public void MouseDrag(Vector3 mousePosition, PlayerId player);
        public void MouseEnter(Vector3 mousePosition, PlayerId player);
        public void MouseExit(Vector3 mousePosition, PlayerId player);
    }
}
