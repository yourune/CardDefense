using System;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Tools
{
    interface ISizeUpdateNotifier
    {
        public event Action<Vector2> OnSizeUpdated;
    }
}
