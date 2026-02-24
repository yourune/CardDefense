namespace BG_Games.Card_Game_Core.Cards.Effects
{
    public enum DurationMode
    {
        TurnStart,
        TurnEnd,
        InvokeRemove
    }

    public interface ITemporaryEffect
    {
        public void Apply(ITroopLogic target, int duration,DurationMode durationMode);

        public void Remove();

        public void NextTurn();
        public void EndTurn();
    }
}
