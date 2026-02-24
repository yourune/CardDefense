using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    [Serializable]
    struct RaceView
    {
        public CardRace Race;
        [Space]
        public Sprite BackView;
        [Space]
        public Sprite HeroView;
        public Sprite UnitView;
        public Sprite SpellView;
        public Sprite InnerFrameView;

        public Sprite GetTypeView(CardInfo info)
        {
            if (info is HeroCardInfo)
            {
                return HeroView;
            }
            else if (info is UnitCardInfo)
            {
                return UnitView;
            }
            else if (info is SpellCardInfo)
            {
                return SpellView;
            }
            else return UnitView;
        }
    }

    [CreateAssetMenu(fileName = "View Config", menuName = "ScriptableObjects/Settings/View")]
    class CardViewSettings:ScriptableObject
    {
        [field: SerializeField] public List<RaceView> ViewPresets { get; private set; }

        public Sprite GetInnerFrameViewOfCard(CardInfo info)
        {
            RaceView race = ViewPresets.Find(preset => info.Race == preset.Race);
            return race.InnerFrameView;
        }

        public Sprite GetFrontViewOfCard(CardInfo info)
        {
            RaceView race = ViewPresets.Find(preset => info.Race == preset.Race);
            return race.GetTypeView(info);
        }

        public Sprite GetBackViewOfCard(CardInfo info)
        {
            RaceView race = ViewPresets.Find(preset => info.Race == preset.Race);
            return race.BackView;
        }

        public Sprite GetBackViewOfDeck(DeckData deck)
        {
            RaceView race = ViewPresets.Find(preset => deck.Race == preset.Race);
            return race.BackView;
        }
    }
}
