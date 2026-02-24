using System;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Factories
{
    public class CardFactory
    {
        private UnitCard _unitPrefab;
        private SpellCard _spellPrefab;

        private DiContainer _container;

        [Inject]
        private CardFactory(DiContainer container,CardFactoriesSettings settings)
        {
            _container = container;
            _unitPrefab = settings.UnitPrefab;
            _spellPrefab = settings.SpellPrefab;
        }

        public Card Create(CardInfo info,Player.Player owner)
        {
            if (info is UnitCardInfo)
            {
                return CreateUnit(info, owner);
            }
            else if (info is SpellCardInfo)
            {
                return CreateSpell(info, owner);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        private Card CreateSpell(CardInfo info, Player.Player owner)
        {
            SpellCardInfo spellInfo = info as SpellCardInfo;
            _container.Inject(spellInfo.LogicFactory);

            SpellCard instance = _container.InstantiatePrefabForComponent<SpellCard>(_spellPrefab,owner.CardsParent);
            instance.InitInfo(spellInfo);

            return instance;
        }

        private Card CreateUnit(CardInfo info, Player.Player owner)
        {
            UnitCardInfo unitInfo = info as UnitCardInfo;
            _container.Inject(unitInfo.LogicFactory);


            UnitCard instance = _container.InstantiatePrefabForComponent<UnitCard>(_unitPrefab,owner.CardsParent);
            instance.InitInfo(unitInfo);

            return instance;
        }
    }
}
