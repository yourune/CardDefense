using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Items
{
    public class DeckCardController:CardController
    {
        public DeckCardInfo DeckCardInfo { get; private set; }

        public void InitInfo(DeckCardInfo info)
        {
            DeckCardInfo = info;

            base.InitInfo(info);
        }
    }
}
