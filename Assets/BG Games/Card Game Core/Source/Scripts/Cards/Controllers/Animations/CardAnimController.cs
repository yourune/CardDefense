using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    public class CardAnimController:MonoBehaviour
    {
        [field:SerializeField] public DragAnimations DragAnimations { get; protected set; }
    }
}
