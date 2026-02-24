using System;
using BG_Games.Card_Game_Core.Player.TurnOrder;
using BG_Games.Card_Game_Core.Tools;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Player
{
    public enum PlayerId
    {
        Player1,
        Player2
    }

    public class Player:MonoBehaviour
    {
        public event Action OnTurnStart;
        public event Action OnTurnEnd;

        [field: SerializeField] public PlayerId ID { get; private set; }
        [field: SerializeField] public Transform CardsParent { get; private set; }
        [field: SerializeField] public CardViewMode CardViewMode { get; private set; }
        [field: SerializeField] public bool PlayCardInputSounds { get; private set; } = true;


        private StateMachine<TurnState> _stateMachine;

        public DrawCard DrawSystem { get; private set; }
        public PlayerDeck Deck { get; private set; }
        public PlayerEnergy Energy { get; private set; }
        public PlayerHand Hand { get; private set; }
        public PlayerHero Hero { get; private set; }

        public bool ScheduledStart { get; private set; } = false;

        [Inject]
        private void Construct(DrawCard drawSystem, PlayerDeck deck, PlayerEnergy energy, PlayerHand hand,PlayerHero hero)
        {
            DrawSystem = drawSystem;
            Deck = deck;
            Energy = energy;
            Hand = hand;
            Hero = hero;
        }

        [Inject]
        private void ConstructTurnOrder(PreTurnState preTurnState, MainTurnState mainTurnState, StartMatchTurnState startMatchTurnState, WaitingTurnState waitingTurnState,MulliganTurnState mulliganTurnState,MatchEndTurnState endTurnState,FirstPreTurnState firstPreTurnState)
        {
            _stateMachine = new StateMachine<TurnState>();


            startMatchTurnState.AssignToPlayerStateMachine(_stateMachine,this);                        
            mulliganTurnState.AssignToPlayerStateMachine(_stateMachine, this);
            firstPreTurnState.AssignToPlayerStateMachine(_stateMachine,this);
            preTurnState.AssignToPlayerStateMachine(_stateMachine,this);
            mainTurnState.AssignToPlayerStateMachine(_stateMachine,this);
            waitingTurnState.AssignToPlayerStateMachine(_stateMachine,this);
            endTurnState.AssignToPlayerStateMachine(_stateMachine,this);
        }

        public void ScheduleStart()
        {
            ScheduledStart = true;
        }

        public void MatchStarted()
        {
            _stateMachine.InitState<StartMatchTurnState>();
        }

        public void NextTurn()
        {
            OnTurnStart?.Invoke();
            _stateMachine.CurrentState.NextTurn();
        }

        public void EndTurn()
        {
            _stateMachine.CurrentState.EndTurn();            
            OnTurnEnd?.Invoke();
        }

        public void MatchEnd()
        {
            _stateMachine.CurrentState.MatchEnd();
        }
    }
}
