using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Card_Game_Core.Tools
{
    class SizeUpdateNotifier:UIBehaviour,ISizeUpdateNotifier
    {
        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = (RectTransform)transform;
        }

        public event Action<Vector2> OnSizeUpdated;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            
            OnSizeUpdated?.Invoke(_rectTransform.rect.size);
        }
    }
}
