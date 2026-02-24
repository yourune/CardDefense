using UnityEngine.Localization;

namespace BG_Games.Card_Game_Core.Systems.Localization
{
    public interface IUpdatesLocalization
    {
        public void SetLocalizedText();
        public void OnLocaleUpdated(Locale locale);
    }
}