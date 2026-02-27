using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private LayerMask targetLayerMask;
    
    private EnemyView currentTarget;
    private Sprite cardImage;
    private Sprite castImage;
    
    public void StartTargeting(Sprite cardImg, Sprite castImg)
    {
        currentTarget = null;
        cardImage = cardImg;
        castImage = castImg;
        
        // Show card image as cursor
        ChangeCursorSystem.Instance.ShowCardImage(cardImage);
    }
    
    public void UpdateTargeting(Vector3 currentPosition)
    {
        EnemyView newTarget = null;
        
        if (Physics.Raycast(currentPosition, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            newTarget = enemyView;
        }
        
        // 타겟이 변경되었을 때만 업데이트
        if (newTarget != currentTarget)
        {
            // 이전 타겟 인디케이터 숨기기
            if (currentTarget != null)
            {
                currentTarget.ShowTargetIndicator(false);
            }
            
            // 새 타겟 인디케이터 보이기 및 커서 변경
            if (newTarget != null)
            {
                newTarget.ShowTargetIndicator(true);
                // 타겟 위에 있으므로 cast image로 변경 (있으면)
                if (castImage != null)
                {
                    ChangeCursorSystem.Instance.ShowCastImage(castImage);
                }
                else
                {
                    ChangeCursorSystem.Instance.ShowCardImage(cardImage);
                }
            }
            else
            {
                // 타겟에서 벗어났으므로 card image로 변경
                ChangeCursorSystem.Instance.ShowCardImage(cardImage);
            }
            
            currentTarget = newTarget;
        }
    }

    public EnemyView EndTargeting(Vector3 endposition)
    {
        EnemyView targetedEnemy = null;
        
        if (Physics.Raycast(endposition, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            targetedEnemy = enemyView;
        }
        
        // 타겟 인디케이터 확실히 숨기기
        if (currentTarget != null)
        {
            currentTarget.ShowTargetIndicator(false);
        }
        
        // 혹시 모를 경우를 대비해 targetedEnemy의 인디케이터도 확인
        if (targetedEnemy != null && targetedEnemy != currentTarget)
        {
            targetedEnemy.ShowTargetIndicator(false);
        }
        
        currentTarget = null;
        cardImage = null;
        castImage = null;
        
        return targetedEnemy;
    }
    
    public void ForceCleanup()
    {
        // 모든 타겟 인디케이터 숨기기
        if (currentTarget != null)
        {
            currentTarget.ShowTargetIndicator(false);
        }
        
        currentTarget = null;
        cardImage = null;
        castImage = null;
    }
}
