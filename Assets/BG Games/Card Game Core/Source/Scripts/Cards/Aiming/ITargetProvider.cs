using System;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    public interface ITargetProvider
    {        
        public event Action<bool> OnAimEnd;
        public void StartAim();
    }
}
