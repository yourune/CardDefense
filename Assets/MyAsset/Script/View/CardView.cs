using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text  _title;
    [SerializeField] private TMP_Text  _mana;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private SpriteRenderer _imageSR;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _dropLayer;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private SpriteRenderer[] allRenderers;

    public Cards Cards { get; private set; }
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private bool isOverDropArea = false;
    
    private bool isMouseOver = false;
    private bool isDragging = false;

     

    public void Setup(Cards cards)
    {
        Cards = cards;
        _title.text = cards.Title;
        _description.text = cards.Description;
        _imageSR.sprite = cards.Image;
        _mana.text = cards.Mana.ToString();
        
        // allRenderers가 설정되지 않았다면 자동으로 찾기
        if (allRenderers == null || allRenderers.Length == 0)
        {
            allRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        
        // Don't process card input if mouse is over UI
        if (IsPointerOverUIElement()) return;
        
        CheckMouseHover();
        HandleMouseInput();
    }
    
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void CheckMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        
        bool nowMouseOver = false;
        
        if (Physics.Raycast(ray, out hit, 100f))
        {
            nowMouseOver = hit.collider != null && hit.collider.gameObject == gameObject;
        }
        
        if (nowMouseOver != isMouseOver)
        {
            isMouseOver = nowMouseOver;
            
            if (isMouseOver)
            {
                OnMouseEnterHandler();
            }
            else
            {
                OnMouseExitHandler();
            }
        }
    }

    private void OnMouseEnterHandler()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        _wrapper.SetActive(false);
        Vector3 hoverPosition = new Vector3(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Cards, hoverPosition);
    }

    private void OnMouseExitHandler()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        _wrapper.SetActive(true);
    }

    private void HandleMouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && isMouseOver)
        {
            OnMouseDownHandler();
        }

        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            OnMouseDragHandler();
        }

        // 마우스 버튼을 뗀 순간
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            OnMouseUpHandler();
            return;
        }
        
        // 안전장치: 마우스 버튼이 눈려있지 않은데 isDragging이 true면 강제 정리
        if (isDragging && !Mouse.current.leftButton.isPressed)
        {
            CleanupCardState();
        }
    }
    
    private bool HasManualPositionTargeting()
    {
        return Cards.AreaEffects != null && Cards.AreaEffects.Count > 0
            && Cards.AreaEffects.FindIndex(ae => ae.PositionTargetMode is ManualPositionTM) >= 0;
    }

    private void OnMouseDownHandler()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        
        isDragging = true;
        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;
        
        // All cards now use drag behavior initially
        Interactions.Instance.PlayerIsDragging = true;
        _wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    private void OnMouseDragHandler()
    {
        if (!Interactions.Instance.PlayerIsDragging) return;
        
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        
        // DropArea 감지
        bool nowOverDropArea = Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, _dropLayer);
        
        if (nowOverDropArea != isOverDropArea)
        {
            isOverDropArea = nowOverDropArea;
            
            if (isOverDropArea)
            {
                // DropArea 진입: 카드 투명 + 커서 이미지 변경
                SetCardAlpha(0f);
                if (Cards.CastImage != null)
                {
                    ChangeCursorSystem.Instance.ShowCastImage(Cards.CastImage);
                }
                else
                {
                    ChangeCursorSystem.Instance.ShowCardImage(Cards.Image);
                }
            }
            else
            {
                // DropArea 탈출: 카드 보임 + 일반 커서
                SetCardAlpha(1f);
                ChangeCursorSystem.Instance.HideCursorImage();
            }
        }
    }

    private void OnMouseUpHandler()
    {
        if (!Interactions.Instance.PlayerCanInteract())
        {
            CleanupCardState();
            return;
        }
        
        isDragging = false;
        
        // Check if card is in drop area and has enough mana
        bool inDropArea = Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, _dropLayer);
        
        if (ManaSystem.Instance.HasEnoughMana(Cards.Mana) && inDropArea)
        {
            bool cardPlayed = false;
            
            // Handle cards with manual target effect (need enemy target)
            if (Cards.ManualTargetEffect != null)
            {
                Vector3 mousePos = MouseUtil.GetMousePositionInWorldSpace(-1);
                if (Physics.Raycast(mousePos, Vector3.forward, out RaycastHit targetHit, 10f, _targetLayer)
                    && targetHit.collider != null
                    && targetHit.transform.TryGetComponent(out EnemyView enemyView))
                {
                    // Set enemy position for any EnemyPositionTM in AreaEffects
                    if (Cards.AreaEffects != null)
                    {
                        foreach (var areaEffect in Cards.AreaEffects)
                        {
                            if (areaEffect?.PositionTargetMode is EnemyPositionTM enemyPosTM)
                            {
                                enemyPosTM.SetEnemyPosition(enemyView);
                            }
                        }
                    }
                    
                    PlayCardGA playCardGA = new (Cards, enemyView);
                    ActionSystem.Instance.Perform(playCardGA);
                    cardPlayed = true;
                }
            }
            // Handle cards with manual position targeting
            else if (HasManualPositionTargeting())
            {
                Vector3 mousePos = MouseUtil.GetMousePositionInWorldSpace(-1);
                // Set position in ManualPositionTM
                foreach (var areaEffect in Cards.AreaEffects)
                {
                    if (areaEffect?.PositionTargetMode is ManualPositionTM manualPosTM)
                    {
                        manualPosTM.SetPosition(mousePos);
                    }
                }
                
                PlayCardGA playCardGA = new (Cards, null);
                ActionSystem.Instance.Perform(playCardGA);
                cardPlayed = true;
            }
            // Handle regular cards (no targeting needed)
            else
            {
                PlayCardGA playCardGA = new (Cards);
                ActionSystem.Instance.Perform(playCardGA);
                cardPlayed = true;
            }
            
            if (cardPlayed)
            {
                // 카드 사용 후 0.2초 쿨다운
                Interactions.Instance.SetCardUsedCooldown();
            }
            else
            {
                // 타겟팅 실패 - 카드 복원
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
                SetCardAlpha(1f);
            }
        }
        else
        {
            // 카드 사용 취소 - 원래 위치로 복귀
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
            SetCardAlpha(1f);
        }
        
        // 커서 복원 및 상태 정리
        ChangeCursorSystem.Instance.HideCursorImage();
        isOverDropArea = false;
        Interactions.Instance.PlayerIsDragging = false;
    }
    
    private void CleanupCardState()
    {
        isDragging = false;
        isOverDropArea = false;
        Interactions.Instance.PlayerIsDragging = false;
        ChangeCursorSystem.Instance.HideCursorImage();
        
        // 카드 원래 위치로 복귀
        if (dragStartPosition != Vector3.zero)
        {
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }
        SetCardAlpha(1f);
    }

    private void SetCardAlpha(float alpha)
    {
        // SpriteRenderer 투명도 설정
        if (allRenderers != null && allRenderers.Length > 0)
        {
            foreach (var renderer in allRenderers)
            {
                if (renderer != null)
                {
                    Color color = renderer.color;
                    color.a = alpha;
                    renderer.color = color;
                }
            }
        }
        
        // TextMeshPro 투명도 설정
        if (_title != null)
        {
            Color titleColor = _title.color;
            titleColor.a = alpha;
            _title.color = titleColor;
        }
        
        if (_mana != null)
        {
            Color manaColor = _mana.color;
            manaColor.a = alpha;
            _mana.color = manaColor;
        }
        
        if (_description != null)
        {
            Color descColor = _description.color;
            descColor.a = alpha;
            _description.color = descColor;
        }
    }
}