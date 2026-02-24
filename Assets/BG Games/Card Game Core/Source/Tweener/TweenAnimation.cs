using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core
{
    public static class TweenAnimation
    {
        public static IEnumerator MoveTo(Transform transform, Vector3 target, float duration, Func<float, float> easeFunction = null, Action onComplete = null)
        {
            Vector3 startPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
            
                float easedT = easeFunction != null ? easeFunction(t) : t;

                transform.position = Vector3.Lerp(startPosition, target, easedT);
                yield return null;
            }

            transform.position = target;
            onComplete?.Invoke();
        }
    
        public static IEnumerator ScaleTo(Transform transform, Vector3 targetScale, float duration, Func<float, float> easeFunction = null, Action onComplete = null)
        {
            Vector3 startScale = transform.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
        
                float easedT = easeFunction != null ? easeFunction(t) : t;

                transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
                yield return null;
            }

            transform.localScale = targetScale;
            onComplete?.Invoke();
        }
    
        public static IEnumerator SliderTo(Slider slider, float targetValue, float duration, Func<float, float> easeFunction = null, Action onComplete = null)
        {
            float startValue = slider.value;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
        
                float easedT = easeFunction != null ? easeFunction(t) : t;

                slider.value = Mathf.Lerp(startValue, targetValue, easedT);
                yield return null;
            }

            slider.value = targetValue;
            onComplete?.Invoke();
        }
    
        public static float EaseInBack(float t)
        {
            float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }
    
        public static float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }
    
        public static float EaseOutElastic(float t)
        {
            float c4 = (2f * Mathf.PI) / 3f;
            return t <= 0f ? 0f : t >= 1f ? 1f
                : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
        }
    
        public static float EaseFlash(float t)
        {
            if (t < 0.5f) return t * 2f;
            return 1f;
        }
    
        public static float Linear(float t) => t;
    }
}
