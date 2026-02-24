using System;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI
{
    class DialogWindow:MonoBehaviour
    {
        [SerializeField] private RectTransform _screen;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _closeButton;

        public event Action OnAccept;
        public event Action OnCancel;
        public event Action OnClose;

        private void Awake()
        {
            _acceptButton.onClick.AddListener(AcceptClick);
            _cancelButton.onClick.AddListener(CancelClick);
            _closeButton.onClick.AddListener(CloseClick);
        }

        public void Open()
        {
            _screen.gameObject.SetActive(true);
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
        }

        private void CloseClick()
        {
            OnClose?.Invoke();
            Close();
        }

        private void CancelClick()
        {
            OnCancel?.Invoke();
        }

        private void AcceptClick()
        {
            OnAccept?.Invoke();
        }
    }
}
