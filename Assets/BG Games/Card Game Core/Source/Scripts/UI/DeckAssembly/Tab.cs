using System;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    class Tab : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Sprite _inactiveSprite;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Image _tabImage;

        public event Action OnClick;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke());
        }

        public void SetActive(bool active)
        {
            _tabImage.sprite = active ? _activeSprite : _inactiveSprite;
        }
    }
}
