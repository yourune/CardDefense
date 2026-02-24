using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Controllers.Animations
{
    public class DragAnimations:MonoBehaviour
    {
        [Header("Drag")]
        [SerializeField] private float _dragSmooth = 5f;
        [SerializeField] private float _dragRotationPower = 40f;
        [SerializeField] private float _dragMaxRotationAngle = 35f;

        private Vector3 _dragVelocity = Vector3.zero;

        public void AnimateDrag(Vector3 dragOffset)
        {
            Vector3 newPos = Vector3.SmoothDamp(transform.position, dragOffset, ref _dragVelocity, _dragSmooth * Time.fixedDeltaTime);
            Vector3 offset = dragOffset - transform.position;

            transform.position = newPos;
            transform.rotation = Quaternion.Euler(Vector3.ClampMagnitude(new Vector3(offset.y * _dragRotationPower, -offset.x * _dragRotationPower), _dragMaxRotationAngle));
        }
    }
}
