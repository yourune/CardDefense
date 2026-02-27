using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text  _title;
    [SerializeField] private TMP_Text  _mana;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private SpriteRenderer _imageSR;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _dropLayer;
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
        
        CheckMouseHover();
        HandleMouseInput();
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

    private void OnMouseDownHandler()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        
        isDragging = true;
        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;
        
        if(Cards.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(Cards.Image, Cards.CastImage);
            
            // ManualTarget 카드도 투명하게 만들고 커서 변경
            SetCardAlpha(0f);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            _wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
            
            // 일반 카드는 클릭 시 보이는 상태 유지 (DropArea 진입 시에만 투명해짐)
        }
    }

    private void OnMouseDragHandler()
    {
        if(Cards.ManualTargetEffect != null)
        {
            // ManualTarget 카드는 타겟팅 업데이트
            Vector3 mousePos = MouseUtil.GetMousePositionInWorldSpace(-1);
            ManualTargetSystem.Instance.UpdateTargeting(mousePos);
            return;
        }
        
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
        
        if(Cards.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if(target!=null && ManaSystem.Instance.HasEnoughMana(Cards.Mana))
            {
                PlayCardGA playCardGA = new(Cards, target);
                ActionSystem.Instance.Perform(playCardGA);
                
                // 카드 사용 후 0.2초 쿨다운
                Interactions.Instance.SetCardUsedCooldown();
            }
            else
            {
                // 타겟팅 실패 시 카드 복원
                SetCardAlpha(1f);
            }
            
            // 커서 복원 및 상태 정리
            ChangeCursorSystem.Instance.HideCursorImage();
            Interactions.Instance.PlayerIsDragging = false;
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Cards.Mana) 
                && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, _dropLayer))
            {
                PlayCardGA playCardGA = new (Cards);
                ActionSystem.Instance.Perform(playCardGA);
                
                // 카드 사용 후 0.2초 쿨다운
                Interactions.Instance.SetCardUsedCooldown();
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
    }
    
    private void CleanupCardState()
    {
        isDragging = false;
        isOverDropArea = false;
        Interactions.Instance.PlayerIsDragging = false;
        ChangeCursorSystem.Instance.HideCursorImage();
        
        // ManualTarget 카드의 경우 ManualTargetSystem 정리
        if (Cards != null && Cards.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.ForceCleanup();
        }
        
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
