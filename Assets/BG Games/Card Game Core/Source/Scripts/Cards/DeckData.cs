using System;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.Cards
{
    [Serializable]
    public struct DeckData
    {
        public int DefaultSize;
        public int MaxDuplicates;

        public string Name;        
        public string Hero;
        public CardRace Race;
        public string[] Cards;

        public DeckData(string name,string hero,CardRace race, int size, int maxDuplicates)
        {
            Name = name;
            Cards = new string[size];
            Hero = hero;
            Race = race;
            DefaultSize = size;
            MaxDuplicates = maxDuplicates;
        }

        public bool IsFull()
        {
            return CountFullness() == Cards.Length;
        }

        public int CountFullness()
        {
            int fullness = 0;

            for (int i = 0; i < Cards.Length; i++)
            {
                if (!string.IsNullOrEmpty(Cards[i]))
                {
                    fullness++;
                }
            }

            return fullness;
        }

        public int CountDuplicates(string id)
        {
            int duplicates = Cards.Count(card => card == id);

            return duplicates;
        }

        public bool AddCard(string newCard, out int duplicatesAfterAdd)
        {
            int index;
            if (GetNextEmptyIndex(out index))
            {
                int duplicates = CountDuplicates(newCard);

                if (duplicates < MaxDuplicates)
                {
                    Cards[index] = newCard;
                    duplicatesAfterAdd = duplicates+1;
                    return true;
                }
            }

            duplicatesAfterAdd = 0;
            return false;
        }

        public bool GetNextEmptyIndex(out int index)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if (string.IsNullOrEmpty(Cards[i]))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }        
        
        public static string GetFullnesAsString(DeckData deck, string splitter = "/")
        {
            int fullness = deck.CountFullness();
            int max = deck.Cards.Length;

            return fullness + splitter + max;
        }
    }
}
