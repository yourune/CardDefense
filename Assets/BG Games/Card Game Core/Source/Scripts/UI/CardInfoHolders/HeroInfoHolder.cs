using System.Linq;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.UI.DeckAssembly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI.CardInfoHolders
{
    public class HeroInfoHolder:CardInfoHolder,ICardHPInfoHolder
    {
        [SerializeField] private HeroPreviewConfig _config;
        [SerializeField] private TMP_Text _HPField;
        [SerializeField] private Image _heroFrame;

        public override void InitInfo(CardInfo info)
        {
            base.InitInfo(info);

            if (info is HeroCardInfo)
            {
                HeroCardInfo heroInfo = info as HeroCardInfo;

                if (_HPField != null)
                {
                    _HPField.text = heroInfo.HP.ToString();
                }

                if (_heroFrame != null)
                {
                    _heroFrame.sprite = _config.Records.First(record => record.Race == info.Race).Frame;
                }
            }
        }

        public void ChangeHP(int value)
        {
            _HPField.text = value.ToString();
        }
    }
}
