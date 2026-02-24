using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.Items
{
    public class CardBarController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private CardInfoHolder _cardInfoHolder;
        [SerializeField] private TMP_Text _cardsCount;

        private DeckCreatorController _deckCreator;

        public string CardId => _cardInfoHolder.Info.Id;

        [Inject]
        private void Construct(DeckCreatorController deckCreator)
        {
            _deckCreator = deckCreator;
        }

        public void SetCountOfDuplicates(int duplicates)
        {
            _cardsCount.text = duplicates.ToString();
        }

        public void InitInfo(CardInfo info)
        {
            _cardInfoHolder.InitInfo(info);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _deckCreator.TakeCard(_cardInfoHolder);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _deckCreator.ReturnCard();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

    }
}
