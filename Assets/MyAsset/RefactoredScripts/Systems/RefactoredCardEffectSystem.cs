using UnityEngine;
using CardDefense.Core.Events;
using CardDefense.Cards;

namespace CardDefense.Systems
{
    /// <summary>
    /// EventBus로 발송된 'CardUsedEvent'를 감지하고
    /// 카드의 Data 안에 들어있는 수많은 블록(Effect)들을 즉시 연속 실행하는 시스템입니다.
    /// </summary>
    public class RefactoredCardEffectSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            EventBus.Subscribe<CardUsedEvent>(OnCardPlayed);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<CardUsedEvent>(OnCardPlayed);
        }

        private void OnCardPlayed(CardUsedEvent evt)
        {
            RefactoredCardData cardData = evt.PlayedCard;
            if (cardData == null) return;

            // TODO: 마나 소모 (ManaSystem.SpendMana(cardData.ManaCost)) 등 처리 가능

            // 한 카드 안에 들어있는 모든 이펙트 장난감 블록들을 순차/동시 실행합니다!
            // 예: 첫 번째 블록(파이어볼 데미지), 두 번째 블록(장판 스폰), 세 번째 블록(마나 회복)
            foreach (var effect in cardData.Effects)
            {
                if (effect != null)
                {
                    // 각 이펙트의 Execute 함수를 호출합니다. targetEntity는 우선 null로 넘깁니다. (장판/광역기가 주력이므로)
                    effect.Execute(evt.CastPosition, null);
                }
            }
        }
    }
}
