using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Tools
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

        private int _page = 1;
        private int _pagesTotal;

        void Start()
        {
            scrollDownButton.onClick.AddListener(ScrollDown);
            scrollUpButton.onClick.AddListener(ScrollUp);

            ISizeUpdateNotifier notifier;
            if (_content.TryGetComponent<ISizeUpdateNotifier>(out notifier))
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

            UpdatePageDisplay();
        }

        private void ScrollDown()
        {
            if (_page >= _pagesTotal)
                return;

            _content.anchoredPosition += new Vector2(0, scrollStep);
            _page++;

            UpdatePageDisplay();
        }

        private void UpdatePageDisplay()
        {
            if (_pageDisplay != null)
            {
                _pageDisplay.text = _page.ToString();
            }

            if (_pageTotalDisplay != null)
            {
                _pageTotalDisplay.text = _addSplitterInTotalPages ? Splitter + _pagesTotal.ToString() : _pagesTotal.ToString();
            }
        }

        private void CalculatePages(Vector2 size)
        {
            float stepRatio = size.y / scrollStep;
            _pagesTotal = (int)stepRatio;

            if (!Mathf.Approximately(size.y % scrollStep,0))
            {
                _pagesTotal++;
            }

            ResetPage();
            UpdatePageDisplay();
        }

        private void ResetPage()
        {
            if (_page > 1)
            {
                _content.anchoredPosition -= new Vector2(0, scrollStep * (_page - 1));                
                _page = 1;
            }

        }
    }
}