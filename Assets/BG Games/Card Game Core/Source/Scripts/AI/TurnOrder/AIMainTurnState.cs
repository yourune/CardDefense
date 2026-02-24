using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Player.TurnOrder;
using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.AI.TurnOrder
{
    class AIMainTurnState:MainTurnState
    {
        private TestBot _testBot;

        public AIMainTurnState(TestBot testBot, PlayerId player, GameTable table, PlayerHand playerHand, TurnSwitch turnSwitch,PlayerHero hero) : base(player, table, playerHand, hero, turnSwitch)
        {
            _testBot = testBot;
        }

        public override void Enter()
        {
            base.Enter();
            _testBot.DoTurn();
        }
    }
}
