using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems.Localization;
using BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters;
using TMPro;
using UnityEngine;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{

    public enum CardType
    {
        All,
        Unit,
        Spell,
        Artifact
    }

    class CollectionFiltersController:MonoBehaviour
    {
        [SerializeField] private CollectionController _collectionController;
        [Space] 
        [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private TMP_Dropdown _raceDropdown;

        private ICollectionFilter _typeFilter;
        private ICollectionFilter _raceFilter;
        
        [SerializeField] private List<string> _typeOptions;
        [SerializeField] private List<string> _raceOptions;

        private void Awake()
        {
            _typeDropdown.onValueChanged.AddListener(SetType);
            _raceDropdown.onValueChanged.AddListener(SetRace);
            PopulateDropdowns();
            
            ResetFilters();
        }

        private void PopulateDropdowns()
        {
            _typeDropdown.ClearOptions();
            _raceDropdown.ClearOptions();

            foreach (var option in _raceOptions)
            {
                _raceDropdown.options
                    .Add(new TMP_Dropdown.OptionData(LocalizationData
                        .GetLocalized(LocalizationData.KeyPrefixes.Option + option, LocalizationData.UILocalizationTable)));    
            }
            
            foreach (var option in _typeOptions)
            {
                _typeDropdown.options
                    .Add(new TMP_Dropdown.OptionData(LocalizationData
                        .GetLocalized(LocalizationData.KeyPrefixes.Option + option, LocalizationData.UILocalizationTable)));    
            }
        }
        
        private void SetRace(int race)
        {
            if (race > 0)
            {
                int unsetRaceOffset = 1;
                race -= unsetRaceOffset;
                SetRaceFilter(new RaceFilter((CardRace)race));
            }
            else
            {
                SetRaceFilter(null);
            }
            
            _raceDropdown.RefreshShownValue();
        }

        private void SetType(int type)
        { 
            if ((CardType)type == CardType.Unit)
            {
                SetTypeFilter(new UnitFilter());
            }
            else if ((CardType)type == CardType.Spell)
            {
                SetTypeFilter(new SpellFilter());
            }
            else
            {
                SetTypeFilter(null);
            }
            
            _typeDropdown.RefreshShownValue();
        }

        public void ResetFilters()
        {
            SetTypeFilter(null);
            SetRaceFilter(null);

            _typeDropdown.SetValueWithoutNotify(0);
            _raceDropdown.SetValueWithoutNotify(0);
        }

        private void SetRaceFilter(ICollectionFilter filter)
        {
            if (_raceFilter != null)
            {
                _collectionController.Filters.Remove(_raceFilter);
            }

            if (filter != null)
            {
                _collectionController.Filters.Add(filter);
                _raceFilter = filter;
            }

            _collectionController.UpdateView();
        }

        private void SetTypeFilter(ICollectionFilter filter)
        {
            if (_typeFilter != null)
            {
                _collectionController.Filters.Remove(_typeFilter);
            }

            if (filter != null)
            {
                _collectionController.Filters.Add(filter);
                _typeFilter = filter;
            }
            
            _collectionController.UpdateView();
        }
    }
}
