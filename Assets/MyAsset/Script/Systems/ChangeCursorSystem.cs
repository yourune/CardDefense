using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChangeCursorSystem : Singleton<ChangeCursorSystem>
{
    [SerializeField] private Image cursorImage;

    private bool isCursorHidden = false;

    protected override void Awake()
    {
        base.Awake();
        HideCursorImage();
    }

    public void ShowCardImage(Sprite cardSprite)
    {
        if (cardSprite == null) return;

        Cursor.visible = false;
        isCursorHidden = true;
        cursorImage.sprite = cardSprite;
        cursorImage.gameObject.SetActive(true);
    }

    public void ShowCastImage(Sprite castSprite)
    {
        if (castSprite == null) return;

        Cursor.visible = false;
        isCursorHidden = true;
        cursorImage.sprite = castSprite;
        cursorImage.gameObject.SetActive(true);
    }

    public void HideCursorImage()
    {
        cursorImage.gameObject.SetActive(false);
        if (isCursorHidden)
        {
            Cursor.visible = true;
            isCursorHidden = false;
        }
    }

    private void Update()
    {
        if (cursorImage.gameObject.activeSelf && Mouse.current != null)
        {
            cursorImage.transform.position = Mouse.current.position.ReadValue();
        }
    }

    public bool IsOverDropArea()
    {
        Vector3 mouseWorldPos = MouseUtil.GetMousePositionInWorldSpace(-1);
        return Physics.Raycast(mouseWorldPos, Vector3.forward, 10f, LayerMask.GetMask("DropArea"));
    }
}
