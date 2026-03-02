using UnityEngine;

/// <summary>
/// System for handling manual position-based targeting (click on ground to place AoE)
/// </summary>
public class ManualPositionTargetSystem : Singleton<ManualPositionTargetSystem>
{
    [SerializeField] private GameObject aoeIndicatorPrefab;
    [SerializeField] private LayerMask groundLayer;
    
    private GameObject currentIndicator;
    private Sprite cardImage;
    private Sprite castImage;
    private float aoeRadius = 3f;
    private ManualPositionTM currentTargetMode;
    private bool isOverValidGround;
    
    public void StartTargeting(Sprite cardImg, Sprite castImg, float radius, ManualPositionTM targetMode)
    {
        cardImage = cardImg;
        castImage = castImg;
        aoeRadius = radius;
        currentTargetMode = targetMode;
        isOverValidGround = false;
        
        // Create AoE indicator
        if (aoeIndicatorPrefab != null)
        {
            currentIndicator = Instantiate(aoeIndicatorPrefab);
            currentIndicator.transform.localScale = Vector3.one * (radius * 2f);
        }
        
        ChangeCursorSystem.Instance.ShowCardImage(cardImage);
    }
    
    public void UpdateTargeting(Vector3 mousePosition)
    {
        // Raycast to ground
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        bool hitGround = Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer);
        
        if (hitGround)
        {
            // Move indicator to hit position
            if (currentIndicator != null)
            {
                currentIndicator.transform.position = hit.point;
            }
            
            // Show cast image when over valid ground
            if (!isOverValidGround)
            {
                isOverValidGround = true;
                if (castImage != null)
                {
                    ChangeCursorSystem.Instance.ShowCastImage(castImage);
                }
                else
                {
                    ChangeCursorSystem.Instance.ShowCardImage(cardImage);
                }
            }
        }
        else
        {
            // Show card image when not over valid ground
            if (isOverValidGround)
            {
                isOverValidGround = false;
                ChangeCursorSystem.Instance.ShowCardImage(cardImage);
            }
        }
    }
    
    public Vector3? EndTargeting(Vector3 endMousePosition)
    {
        Vector3? targetPosition = null;
        
        // Get final position
        Ray ray = Camera.main.ScreenPointToRay(endMousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            targetPosition = hit.point;
            
            // Set position in target mode
            if (currentTargetMode != null)
            {
                currentTargetMode.SetPosition(hit.point);
            }
        }
        
        // Cleanup
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
            currentIndicator = null;
        }
        
        ChangeCursorSystem.Instance.HideCursorImage();
        currentTargetMode = null;
        
        return targetPosition;
    }
    
    public void ForceCleanup()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
            currentIndicator = null;
        }
        
        currentTargetMode = null;
        ChangeCursorSystem.Instance.HideCursorImage();
    }
}
