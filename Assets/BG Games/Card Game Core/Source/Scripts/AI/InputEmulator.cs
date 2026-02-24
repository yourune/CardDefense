using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.AI
{
    public class InputEmulator:MonoBehaviour
    {
        private Camera _mainCamera;
        private PlayerId _playerId;
        private AimSystemAI _aimSystemAi;

        [Inject]
        private void Construct(PlayerId playerId,AimSystemAI aimSystemAi)
        {
            _playerId = playerId;
            _aimSystemAi = aimSystemAi;
        }


        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void MoveCard(Card card, Vector3 targetPos)
        {
            Vector3 cardScreenPos = _mainCamera.WorldToScreenPoint(card.transform.position);
            Vector3 targetScreenPos = _mainCamera.WorldToScreenPoint(targetPos);

            card.MouseDown(cardScreenPos,_playerId);
            card.MouseDrag(targetScreenPos, _playerId);
            card.MouseUp(targetScreenPos, _playerId);
        }

        public void PlayTargetedSpell(SpellCard card, Vector3 targetPos, Vector3 spellTarget)
        {
            _aimSystemAi.PreSetTarget(spellTarget);
            MoveCard(card,targetPos);
        }

        public void PlaySpell(SpellCard card, Vector3 targetPos)
        {
            MoveCard(card,targetPos);
        }

        public bool AttackByCard(UnitCard card, Vector3 target)
        {
            _aimSystemAi.PreSetTarget(target);
            card.MouseUp(card.Position, _playerId);

            return true;
        }

        public bool ApplyHeroAbillity(Hero hero, Vector3 target)
        {
            _aimSystemAi.PreSetTarget(target);
            hero.Abillity.MouseUp(hero.Abillity.transform.position,_playerId);

            return true;
        }
    }
}
