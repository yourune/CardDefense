using System;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    class MockTargetProvider:ITargetProvider
    {
        public event Action<bool> OnAimEnd;
        public void StartAim()
        {
            OnAimEnd?.Invoke(true);
        }
    }
}
