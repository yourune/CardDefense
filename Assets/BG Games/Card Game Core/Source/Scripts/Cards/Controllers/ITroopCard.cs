namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public interface ITroopCard:ICard
    {
        public ITroopLogic Logic{ get; }
    }
}
