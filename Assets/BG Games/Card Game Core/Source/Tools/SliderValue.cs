using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Tools
{
    class SliderValue:MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _text;
        [Space] 
        [SerializeField] private bool _inPercents = true;
        [SerializeField] private string _percentSymbol = "%";

        private void Awake()
        {
            _slider.onValueChanged.AddListener(ChangeValue);

            ChangeValue(_slider.value);
        }

        private void ChangeValue(float value)
        {
            if (_inPercents)
            {
                _text.text = ((int)(value * 100)).ToString() + _percentSymbol;
            }
            else
            {
                _text.text = value.ToString();
            }

        }
    }
}
