using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace BG_Games.Card_Game_Core.Systems.Localization
{
    public class LocalizationSystem : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;

        private void Awake()
        {
            string language = PlayerPrefs.GetString(LocalizationData.PlayerPrefsKey, "English");
            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales.Find(locale =>
                    locale.Identifier.CultureInfo.DisplayName == language);
        }

        public void ChangeLocale(int index)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales
                .Find(locale => locale.Identifier.CultureInfo.DisplayName == _dropdown.options[index].text);
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString(LocalizationData.PlayerPrefsKey, locale.Identifier.CultureInfo.DisplayName);
        }

        public void PopulateDropdown(TMP_Dropdown languageDropdown)
        {
            _dropdown = languageDropdown;
            _dropdown.onValueChanged.AddListener(ChangeLocale);

            languageDropdown.options.Clear();
            var currentLocale = LocalizationSettings.SelectedLocale;
            for (var index = 0; index < LocalizationSettings.AvailableLocales.Locales.Count; index++)
            {
                TMP_Dropdown.OptionData data;

                if (index == 0)
                {
                    data = new TMP_Dropdown.OptionData(currentLocale.Identifier.CultureInfo.DisplayName);
                    languageDropdown.options.Add(data);
                    continue;
                }

                if (LocalizationSettings.AvailableLocales.Locales[index] == currentLocale)
                {
                    languageDropdown.options.Add(new TMP_Dropdown.
                        OptionData(LocalizationSettings.AvailableLocales.Locales[0].Identifier.CultureInfo
                            .DisplayName));
                    continue;
                }

                var locale = LocalizationSettings.AvailableLocales.Locales[index];
                languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.DisplayName));
            }
        }
    }
}