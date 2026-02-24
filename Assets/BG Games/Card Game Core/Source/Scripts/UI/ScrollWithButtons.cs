using BG_Games.Card_Game_Core.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI
{
    public class ScrollWithButtons : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button scrollDownButton;
        [SerializeField] private Button scrollUpButton;           
        [SerializeField] private float scrollStep = 100f;     

        [Space]
        [SerializeField] [Tooltip("Can be NULL")] private TMP_Text _pageDisplay;
        [SerializeField] [Tooltip("Can be NULL")] private TMP_Text _pageTotalDisplay;
        [SerializeField] private bool _addSplitterInTotalPages;

        private const string Splitter = "/";

        private int _page;
        private int _pagesTotal;

        void Start()
        {
            scrollDownButton.onClick.AddListener(ScrollDown);
            scrollUpButton.onClick.AddListener(ScrollUp);

            ISizeUpdateNotifier notifier;
            if (_content.TryGetComponent(out notifier))
            {
                notifier.OnSizeUpdated += CalculatePages;
            }
            
            CalculatePages(_content.rect.size);
        }

        private void ScrollUp()
        {
            if (_page == 1)
                return;

            _content.anchoredPosition -= new Vector2(0, scrollStep);
            _page--;

            UpdatePagesDisplay();
        }

        private void ScrollDown()
        {
            if (_page == _pagesTotal)
                return;

            _content.anchoredPosition += new Vector2(0, scrollStep);
            _page++;

            UpdatePagesDisplay();
        }

        private void UpdatePagesDisplay()
        {
            if (_pageDisplay != null)
            {
                _pageDisplay.text = _page.ToString();
            }

            if (_pageTotalDisplay != null)
            {
                _pageTotalDisplay.text = _addSplitterInTotalPages ? Splitter + _pagesTotal : _pagesTotal.ToString();
            }
        }

        private void CalculatePages(Vector2 size)
        {
            float stepRatio = size.y / scrollStep;
            int pages = (int)stepRatio;

            if (!Mathf.Approximately(stepRatio,0))
            {
                pages++;
            }

            _pagesTotal = pages;
            UpdatePagesDisplay();
        }
    }
}