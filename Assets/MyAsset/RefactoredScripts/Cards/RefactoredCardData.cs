using System.Collections.Generic;
using UnityEngine;
using CardDefense.Core.Effects;
using SerializeReferenceEditor; // 기존 프로젝트에서 발견된 플러그인 활용

namespace CardDefense.Cards // 네임스페이스 통일
{
    [CreateAssetMenu(menuName = "CardDefense/RefactoredCardData", fileName = "NewCard")]
    public class RefactoredCardData : ScriptableObject
    {
        [Header("Card Profile")]
        public string Title = "New Card";
        [TextArea] public string Description = "Card effect description";
        public Sprite Image;
        public Sprite CastImage;
        public int ManaCost = 1;

        [Header("Targeting Settings")]
        [Tooltip("물리적 위치를 선택해야 하는 카드인가? (장판, 파이어볼 등)")]
        public bool RequiresPositionTarget = true;
        
        [Tooltip("특정 적을 직접 클릭해야 하는 카드인가? (단일 디버프 등)")]
        public bool RequiresEntityTarget = false;

        // [SR] 속성은 기존 프로젝트에 있는 SerializeReferenceEditor 의존성을 활용합니다.
        // 이것을 사용하면 Unity Inspector에서 인터페이스(IEngineEffect)를 리스트로 넣을 수 있습니다.
        [Header("Execution Effects (블록 조립)")]
        [SR] [SerializeReference] 
        public List<IEngineEffect> Effects = new List<IEngineEffect>();
    }
}
