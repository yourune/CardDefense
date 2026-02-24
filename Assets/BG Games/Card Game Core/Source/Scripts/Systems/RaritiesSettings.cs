using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    [Serializable]
    public struct RarityConfig
    {
        public CardRarity Rarity;
        public Sprite Sprite;

        public string Label => Rarity.ToString();
    }

    [CreateAssetMenu(fileName = "RaritySprites",menuName = "ScriptableObjects/Settings/Rarity")]
    public class RaritiesSettings:ScriptableObject
    {
        [field: SerializeField]public List<RarityConfig> Rarities { get; private set; }

        public Sprite GetRaritySprite(CardRarity rarity)
        {
            RarityConfig config = FindConfig(rarity);
            return config.Sprite;
        }

        public string GetRarityLabel(CardRarity rarity)
        {
            RarityConfig config = FindConfig(rarity);
            return config.Label;
        }

        public RarityConfig FindConfig(CardRarity rarity)
        {
            if (Rarities.Exists(config => config.Rarity == rarity))
            {
                RarityConfig config = Rarities.Find(config => config.Rarity == rarity);
                return config;
            }
            else
            {
                throw new Exception($"Rarity config not found for rarity: {rarity}");
            }
        }
    }
}
