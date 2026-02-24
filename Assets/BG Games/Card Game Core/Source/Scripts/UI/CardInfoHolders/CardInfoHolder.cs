using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.Localization;
using BG_Games.Card_Game_Core.Tools;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.CardInfoHolders
{
    public enum CardViewMode
    {
        Front,
        Back
    }

    public class CardInfoHolder : MonoBehaviour,ICardInfoHolder, IUpdatesLocalization
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Image _cardTemplate;
        [Header("Content")]
        [SerializeField] private Image _image;
        [SerializeField] private Image _rarityImage;
        [SerializeField] private Image _innerFrame;
        [SerializeField] private TMP_Text _rarityText;
        [SerializeField] private TMP_Text _nameField;
        [SerializeField] private TMP_Text _costField;
        [SerializeField] private TMP_Text _descriptionField;

        private Vector3 _savedPosition;
        private Quaternion _savedRotation;
        private Vector3 _savedScale;
        
        public Transform Transform => transform;
        public CardInfo Info { get; private set; }

        private RaritiesSettings _raritiesesSettings;
        private CardViewSettings _viewSettings;

        [Inject]
        private void Construct(RaritiesSettings settings, CardViewSettings viewSettings)
        {
            _raritiesesSettings = settings;
            _viewSettings = viewSettings;
        }

        protected virtual void Awake()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleUpdated;
        }

        public virtual void InitInfo(CardInfo info)
        {
            Info = info;

            if (_costField != null)
                _costField.text = Info.Cost.ToString();

            if (_image != null)
                _image.sprite = info.Image;

            if (_rarityImage != null)
                _rarityImage.sprite = _raritiesesSettings.GetRaritySprite(info.Rarity);

            if (_rarityText != null)
                _rarityText.text = _raritiesesSettings.GetRarityLabel(info.Rarity);

            if (_innerFrame != null)
                _innerFrame.sprite = _viewSettings.GetInnerFrameViewOfCard(info);
            
            SetLocalizedText();
        }

        public void SetCost(int cost)
        {
            _costField.text = cost.ToString();
        }
        public void SetLayer(string layerName)
        {
            _canvas.sortingLayerName = layerName;
        }
        
        public void SetMode(CardViewMode mode)
        {
            if (mode == CardViewMode.Front)
            {
                if (_cardTemplate != null)
                {
                    _cardTemplate.sprite = _viewSettings.GetFrontViewOfCard(Info);
                }

                if (_container != null)
                {
                    _container.gameObject.SetActive(true);
                }

            }
            else
            {
                if (_cardTemplate != null)
                {
                    _cardTemplate.sprite = _viewSettings.GetBackViewOfCard(Info);
                }

                if (_container != null)
                {
                    _container.gameObject.SetActive(false);
                }
            }
        }

        public void TransformView(Vector3 targetPosition, Quaternion targetRotation, Vector3 deltaScale)
        {
            SaveTransform();

            _canvas.transform.position = targetPosition;
            _canvas.transform.rotation = targetRotation;
            _canvas.transform.localScale = _canvas.transform.localScale.MultiplyByCoordinates(deltaScale);
        }

        public void ReturnView()
        {
            _canvas.transform.localPosition = _savedPosition;
            _canvas.transform.rotation = _savedRotation;
            _canvas.transform.localScale = _savedScale;
        }

        private void SaveTransform()
        {
            _savedPosition = _canvas.transform.localPosition;
            _savedRotation = _canvas.transform.rotation;
            _savedScale = _canvas.transform.localScale;
        }

        public void SetLocalizedText()
        {
            if (_nameField != null)
                _nameField.text = Info.Name.GetLocalizedString();

            if (_descriptionField != null)
                _descriptionField.text = Info.Description.GetLocalizedString();
        }

        public void OnLocaleUpdated(Locale locale)
        {
            SetLocalizedText();
        }
    }
}