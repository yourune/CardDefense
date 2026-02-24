using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Factories
{
    public class CardControllerFactory:MonoBehaviour
    {
        [SerializeField] private DeckCardController _unitPrefab;
        [SerializeField] private DeckCardController _spellPrefab;
        [SerializeField] private DeckCardController _artifactPrefab;

        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public DeckCardController Create(CardInfo info, Transform parent)
        {
            if (info is UnitCardInfo)
            {
                return Create(info, _unitPrefab, parent);
            }
            else if (info is SpellCardInfo)
            {
                return Create(info, _spellPrefab, parent);
            }

            return null;
        }

        private DeckCardController Create(CardInfo info,DeckCardController prefab,Transform parent)
        {
            DeckCardController instance = _instantiator.InstantiatePrefabForComponent<DeckCardController>(prefab.gameObject,parent);
            instance.InitInfo(info as DeckCardInfo);
            return instance;
        }
    }
}
