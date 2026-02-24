using BG_Games.Card_Game_Core.Cards.Info;
using TMPro;
using UnityEngine;

namespace BG_Games.Card_Game_Core.UI.CardInfoHolders
{
    public class UnitInfoHolder : PurchasedCardInfoHolder, ICardHPInfoHolder
    {
        [Header("Context")]
        [SerializeField] private TMP_Text _HPField;
        [SerializeField] private TMP_Text _DPField;

        public override void InitInfo(CardInfo info)
        {
            base.InitInfo(info);

            if (info is UnitCardInfo)
            {
                UnitCardInfo unitInfo = info as UnitCardInfo;

                if (_HPField != null)
                    _HPField.text = unitInfo.HP.ToString();

                if (_DPField != null)
                    _DPField.text = unitInfo.DP.ToString();
            }

            if (info is HeroCardInfo)
            {
                HeroCardInfo heroInfo = info as HeroCardInfo;

                if (_HPField != null)
                    _HPField.text = heroInfo.HP.ToString();
            }

        }

        public void ChangeHP(int value)
        {
            _HPField.text = value.ToString();
        }

        public void ChangeDP(int value)
        {
            _DPField.text = value.ToString();
        }

    }
}