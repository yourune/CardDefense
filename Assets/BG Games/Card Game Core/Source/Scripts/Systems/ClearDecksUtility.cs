using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Systems
{
    class ClearDecksUtility:MonoBehaviour
    {
        [SerializeField] private string DecksVersion = "0.2";

        private const string DecksVersionKey = "DecksVersion";

        [Inject]private IDeckSaveSystem _deckSaveSystem;

        private void Awake()
        {
            if (PlayerPrefs.GetString(DecksVersionKey, "-") != DecksVersion)
            {
                Clear();
                PlayerPrefs.SetString(DecksVersionKey, DecksVersion);
            }
        }

        private void Clear()
        {
            string[] deckNames = _deckSaveSystem.LoadDeckNames();

            foreach (var deckName in deckNames)
            {
                _deckSaveSystem.DeleteDeck(deckName);
            }
        }
    }
}
