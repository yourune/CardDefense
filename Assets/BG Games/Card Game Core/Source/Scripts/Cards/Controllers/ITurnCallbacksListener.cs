namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public interface ITurnCallbacksListener
    {
        public void NextTurn();
        public void EndTurn();
    }
}
