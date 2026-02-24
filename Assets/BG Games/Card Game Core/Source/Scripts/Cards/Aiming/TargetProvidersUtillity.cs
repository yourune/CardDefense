using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    public static class TargetProvidersUtillity
    {
        public static void StartAimWithPos(ITargetProvider targetProvider, Vector3 pos)
        {
            if (targetProvider is IInputNeedyTargetProvider inputNeedy)
            {
                inputNeedy.StartAim(pos);
            }
            else
            {
                targetProvider.StartAim();
            }
        }
    }
}
