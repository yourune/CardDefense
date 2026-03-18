using UnityEngine;
using System.Collections.Generic;

namespace CardDefense.Cards
{
    [CreateAssetMenu(fileName = "NewRefactoredDeck", menuName = "Data/Refactored Deck Data")]
    public class RefactoredDeckData : ScriptableObject
    {
        [Tooltip("이 덱을 구성하는 카드 데이터 리스트 (런타임에 뷰로 변환됩니다.)")]
        public List<RefactoredCardData> Cards = new List<RefactoredCardData>();
    }
}
