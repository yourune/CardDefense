using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    public abstract class DynamicCardsContainer : MonoBehaviour
    {
        [SerializeField][Tooltip("Can be NULL")] protected ContentSizeFitter _sizeFitter;

        protected abstract LayoutGroup ContentHolder { get; }

        protected int TakenCardIndex;
        protected ICardInfoHolder TakenCard;

        protected ITopLayerHauler TopLayerHauler;

        [Inject]
        protected virtual void Construct(ITopLayerHauler topLayerHauler)
        {
            TopLayerHauler = topLayerHauler;
        }

        public virtual void TakeCard(ICardInfoHolder card)
        {
            if (_sizeFitter != null)
            {
                _sizeFitter.enabled = false;
            }

            TakenCardIndex = card.Transform.GetSiblingIndex();
            TakenCard = card;
            ContentHolder.enabled = false;
            TopLayerHauler.BringToTopLayer(card.Transform);
        }

        public virtual void ReturnCard()
        {
            if (_sizeFitter != null)
            {
                _sizeFitter.enabled = true;
            }

            TakenCard.Transform.parent = ContentHolder.transform;
            ContentHolder.enabled = true;
            TakenCard.Transform.SetSiblingIndex(TakenCardIndex);
        }
    }
}