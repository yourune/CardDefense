using System.Collections.Generic;
using System.IO;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Tools;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    public class DeckLocalSave:IDeckSaveSystem
    {
        public string DecksDirectory => "Decks";
        public string DeckDirectoryFullPath => Path.Combine(Application.persistentDataPath, DecksDirectory);

        public void SaveDeck(DeckData deck)
        {
            string filePath = GetFullPathFromDeckName(deck.Name);

            string serializedDeck = JsonUtility.ToJson(deck);

            FileSave.Save(serializedDeck, filePath);
        }

        public bool LoadDeck(string deckName, out DeckData deck)
        {
            string filePath = GetFullPathFromDeckName(deckName);

            string serializedDeck;

            if (FileSave.Load(filePath, out serializedDeck))
            {
                deck = JsonUtility.FromJson<DeckData>(serializedDeck);
                return true;
            }
            else
            {
                deck = default(DeckData);
                return false;
            }
        }

        public string[] LoadDeckNames()
        {
            if (Directory.Exists(DeckDirectoryFullPath))
            {
                string[] filePaths = Directory.GetFiles(DeckDirectoryFullPath);

                string[] fileNames = new string[filePaths.Length];
                for (int i = 0; i < filePaths.Length; i++)
                {
                    fileNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
                }

                return fileNames;
            }

            return new string[0];
        }

        public DeckData[] LoadDecks()
        {
            string[] names = LoadDeckNames();
            List<DeckData> decks = new List<DeckData>();

            for (int i = 0; i < names.Length; i++)
            {
                DeckData loadingResult;

                if (LoadDeck(names[i], out loadingResult))
                {
                    decks.Add(loadingResult);
                }
            }

            return decks.ToArray();
        }

        public void DeleteDeck(string deckName)
        {
            string fullPath = GetFullPathFromDeckName(deckName);
            FileSave.DeleteFile(fullPath);
        }

        private string GetFullPathFromDeckName(string name)
        {
            string fileName = name + ".json";
            string path = Path.Combine(DeckDirectoryFullPath, fileName);

            return path;
        }
    }
}
