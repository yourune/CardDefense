using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.UnitLogic;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.AI
{
    public class TestBot:MonoBehaviour
    {
        [SerializeField] private float _preTurnWaitTime = 0.5f;
        [SerializeField] private float _actionsDelay = 0.8f;
        [SerializeField] [Range(0, 1)] private float _notKillAttackProbability = 0.1f;

        private PlayerRegistry _playerRegistry;
        private PlayerHand _hand;
        private TableSide _myTableSide;
        private TableSide _opponentTableSide;
        private PlayerEnergy _energy;
        private InputEmulator _inputEmulator;
        private Player.Player _player;
        private AimSystemAI _aimSystemAI;

        [Inject]
        private void Construct(PlayerId owner, GameTable table,PlayerHand hand,PlayerEnergy energy,InputEmulator inputEmulator,PlayerRegistry playerRegistry,AimSystemAI aimSystemAi)
        {
            _playerRegistry = playerRegistry;
            _player = playerRegistry.GetPlayerByID(owner);
            _myTableSide = table.GetMyTableSide(owner);
            _opponentTableSide = table.GetOpponentTableSide(owner);
            _hand = hand;
            _energy = energy;
            _inputEmulator = inputEmulator;
            _aimSystemAI = aimSystemAi;

            _aimSystemAI.OnNotSpecifiedTarget += FindRandomTarget;
        }

        private Vector3 FindRandomTarget(IInputNeedyTargetProvider arg)
        {
            Vector3 target;
            if (TryGetRandomTarget(arg,out target))
            {
                return target;
            }

            return Vector3.zero;
        }

        public void DoTurn()
        {
            StartCoroutine(DecisionMaking());
        }

        private IEnumerator DecisionMaking()
        {
            yield return new WaitForSeconds(_preTurnWaitTime);

            yield return StartCoroutine(CheckAbillity(_player.Hero.Card.HeroLogic.HeroInfo.BeforePlacingProbability));

            yield return StartCoroutine(PlacingCards());

            yield return StartCoroutine(CheckAbillity(_player.Hero.Card.HeroLogic.HeroInfo.BeforeAttackingProbability));

            yield return StartCoroutine(AttackCards());

            yield return StartCoroutine(CheckAbillity(_player.Hero.Card.HeroLogic.HeroInfo.AfterAttackingProbability));

            _player.EndTurn();
        }

        private IEnumerator AttackCards()
        {
            ReadOnlyCollection<ITroopCard> myCards = _myTableSide.TroopCards;
            ReadOnlyCollection<ITroopCard> enemyCards = _opponentTableSide.TroopCards;
            Hero enemyHero = _playerRegistry.GetOpponentOfPlayer(_player.ID).Hero.Card;

            if (myCards.Count == 0)
                yield break;

            List<ITroopCard> attackCards = myCards.ToList();
            
            foreach (ITroopCard card in attackCards)
            {
                if (card != null && card is UnitCard)
                {
                    AttackByCard(card as UnitCard, enemyCards,enemyHero);
                    yield return new WaitForSeconds(_actionsDelay);
                }
            }
        }
        private IEnumerator PlacingCards()
        {
            if (_hand.Cards.Count == 0)
               yield break;

            List<int> cardCosts = (from card in _hand.Cards
                                   select card.Cost).ToList();

            List<int> cardIndices = MaxSumSetSelector.FindSubsetWithMaxSum(cardCosts, _energy.Balance);
            cardIndices = cardIndices.OrderByDescending(index => _hand.Cards[index].Cost).ToList();

            List<Card> cardsToPlace = new List<Card>();
            foreach (int index in cardIndices)
            {
                cardsToPlace.Add(_hand.Cards[index]);
            }

            foreach (Card card in cardsToPlace)
            {
                if (card is SpellCard || _myTableSide.CanAddCard())
                {
                    PlaceCard(card);

                    yield return new WaitForSeconds(_actionsDelay);
                }
            }
        }

        private void AttackByCard(UnitCard card,ReadOnlyCollection<ITroopCard> enemyUnits, Hero enemyHero)
        {
            Vector3 target;
            if (GetTargetForUnit(card,enemyUnits,enemyHero,out target) == false)
                return;

            _inputEmulator.AttackByCard(card, target);
        }

        private bool GetTargetForUnit(UnitCard card,ReadOnlyCollection<ITroopCard> enemyUnits, Hero enemyHero, out Vector3 target)
        {
            if (enemyUnits.Count == 0)
            {
                if (enemyHero != null)
                {
                    target = enemyHero.Position;
                    return true;
                }
                target = Vector3.zero;
                return false;
            }

            UnitCard unitTarget = default(UnitCard);
            int lastBiggestDP = 0;

            foreach (UnitCard unit in enemyUnits)
            {
                if (CanKill(card.UnitLogic,unit.UnitLogic, out _))
                {
                    if (unit.UnitLogic.DP > lastBiggestDP)
                    {
                        unitTarget = unit;
                    }
                }
            }

            if (unitTarget == null || unitTarget.UnitLogic is StealthLogic)
            {
                if (Random.Range(0,(float)1) < _notKillAttackProbability)
                {
                    
                }

                target = enemyHero.Position;
                return true;
            }
            else
            {
                target = unitTarget.Position;
                return true;
            }
        }

        private bool CanKill(IUnitLogic card, IUnitLogic target, out int hpAfterAttack)
        {
            bool kill = card.DP >= target.HP;
            hpAfterAttack = target.WillCounterattack(card)
                ? card.HP - card.CalculateCounterattackDamage(card)
                : card.HP;

            return kill;
        }

        private void PlaceCard(Card card)
        {
            int indexLocation = Random.Range(0, _hand.Cards.Count + 1);
            Vector3 targetLocation = _myTableSide.transform.position;

            if (_myTableSide.Cards.Count > 0)
            {
                if (indexLocation == 0)
                {
                    targetLocation.x = _hand.Cards[0].transform.position.x - 0.1f;
                }
                else if (indexLocation == _hand.Cards.Count)
                {
                    targetLocation.x = _hand.Cards[^1].transform.position.x + 0.1f;
                }
                else
                {
                    Vector3 prevCardPos = _hand.Cards[indexLocation - 1].transform.position;
                    Vector3 nextCardPos = _hand.Cards[indexLocation].transform.position;

                    targetLocation.x = Mathf.Lerp(prevCardPos.x, nextCardPos.x, 0.5f);
                }
            }

            _inputEmulator.MoveCard(card, targetLocation);

        }

        private IEnumerator CheckAbillity(float probability)
        {
            float random = Random.Range(0, (float)1);

            IHeroLogic heroLogic = _player.Hero.Card.HeroLogic;
            
            if (random <= probability && heroLogic.AbillityAvailable && _energy.CanSpend(heroLogic.AbillityCost))
            {
                Vector3 target = Vector3.zero;

                ITargetProvider targetProvider = _player.Hero.Card.TargetProvider;

                if (targetProvider is IInputNeedyTargetProvider)
                {
                    IInputNeedyTargetProvider inputTargetProvider = targetProvider as IInputNeedyTargetProvider;
                    
                    Vector3 targetPos;

                    if (TryGetRandomTarget(inputTargetProvider, out targetPos))
                    {
                        _inputEmulator.ApplyHeroAbillity(_player.Hero.Card, targetPos);
                        yield return new WaitForSeconds(_actionsDelay);
                    }
                }
                else
                {
                    _inputEmulator.ApplyHeroAbillity(_player.Hero.Card, Vector3.zero);
                    yield return new WaitForSeconds(_actionsDelay);
                }

            }
        }

        private bool TryGetRandomTarget(IInputNeedyTargetProvider targetProvider, out Vector3 target)
        {
            if (targetProvider.TargetType == TargetType.Ally)
            {
                if (_myTableSide.Cards.Count == 0)
                {
                    target = Vector3.zero;
                    return false;
                }

                int randomTarget = Random.Range(0, _myTableSide.Cards.Count);

                target = _myTableSide.Cards[randomTarget].Position;
                return true;
            }
            else if (targetProvider.TargetType == TargetType.Enemy)
            {
                if (_opponentTableSide.Cards.Count == 0)
                {
                    target = _playerRegistry.GetOpponentOfPlayer(_player.ID).Hero.Card.Position;
                    return true;
                }

                int randomTarget = Random.Range(0, _opponentTableSide.Cards.Count);
                target = _opponentTableSide.Cards[randomTarget].Position;
                return true;
            }
            else
            {
                target = Vector3.zero;
                return false;
            }
        }
    }
}
