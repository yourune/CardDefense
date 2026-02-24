using System;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems.Localization;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    public class BattleLog : MonoBehaviour
    {
        public event Action<string, PlayerId> OnAddedMessage;
        
        private string _battleLogTable;
        
        private void Awake()
        {
            _battleLogTable = LocalizationData.BattleLogLocalizationTable;
        }

        private string GetAuthorTitle(PlayerId owner)
        {
            return owner == PlayerId.Player1
                ? LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogPlayerName, _battleLogTable)
                : LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogOpponentName, _battleLogTable);
        }


        public void LogPlayCard(CardInfo card, PlayerId owner)
        {
            string player = GetAuthorTitle(owner);
            string message = LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogPlayCard, _battleLogTable);
            message = String.Format(message, player, card.Name.GetLocalizedString());

            OnAddedMessage?.Invoke(message, owner);
        }

        public void LogAttack(CardInfo card, CardInfo target, PlayerId owner)
        {
            string message = LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogAttack, _battleLogTable);
            message = String.Format(message, card.Name.GetLocalizedString(), target.Name.GetLocalizedString());

            OnAddedMessage?.Invoke(message, owner);
        }

        public void LogHeroAbility(HeroCardInfo hero, CardInfo target, PlayerId owner)
        {
            string message = LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogPlayHeroAbilityOnCard, _battleLogTable);
            message = String.Format(message, hero.Name.GetLocalizedString(), hero.AbillityName.GetLocalizedString(),
                target.Name.GetLocalizedString());

            OnAddedMessage?.Invoke(message, owner);
        }

        public void LogHeroAbility(HeroCardInfo hero, PlayerId owner)
        {
            string message = LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogPlayHeroAbility, _battleLogTable);
            message = String.Format(message, hero.Name.GetLocalizedString(), hero.AbillityName.GetLocalizedString());

            OnAddedMessage?.Invoke(message, owner);
        }

        public void LogPlaySpell(CardInfo card, CardInfo target, PlayerId owner)
        {
            string player = GetAuthorTitle(owner);

            string message = LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.LogSpell, _battleLogTable);
            message = String.Format(message, player, card.Name.GetLocalizedString(), target.Name.GetLocalizedString());

            OnAddedMessage?.Invoke(message, owner);
        }
    }
}