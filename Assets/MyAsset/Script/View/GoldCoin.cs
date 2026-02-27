using UnityEngine;

// Simple gold coin visual - attach to a sprite GameObject
public class GoldCoin : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
    
    // You can add rotation animation here
    private void Update()
    {
        transform.Rotate(Vector3.forward, 180f * Time.deltaTime);
    }
}
