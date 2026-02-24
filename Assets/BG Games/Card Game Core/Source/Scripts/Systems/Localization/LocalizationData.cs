using BG_Games.Card_Game_Core.Tools;
using UnityEngine.Localization.Settings;

namespace BG_Games.Card_Game_Core.Systems.Localization
{
    public static class LocalizationData
    {
        public const string CardLocalizationTable = "Card Localization";
        public const string UILocalizationTable = "UI Localization";
        public const string BattleLogLocalizationTable = "Battle Log Localization";
        public const string PlayerPrefsKey = "LocalizationKey";

        public static class KeyPrefixes
        {
            public const string LogAttack = "Log_Attack";
            public const string LogPlayerName = "Log_PlayerName";
            public const string LogOpponentName = "Log_OpponentName";
            public const string LogSpell = "Log_Spell";
            public const string LogPlayCard = "Log_PlayCard";
            public const string LogPlayHeroAbility = "Log_HeroAbility";
            public const string LogPlayHeroAbilityOnCard = "Log_HeroAbilityOnCard";
            
            public const string RaceLabel = "RaceLabel_"; 
            public const string Option = "Option_";
            public const string Label = "Label_";
            
            public const string LabelDeck = "Label_Deck";
        }
            
        
        public static string GetLocalized(string entryKey, string tableReference)
        {
            var entry = entryKey.RemoveWhitespaces();
        
            return LocalizationSettings.StringDatabase
                .GetLocalizedString(tableReference, entry);
        }
    }
}