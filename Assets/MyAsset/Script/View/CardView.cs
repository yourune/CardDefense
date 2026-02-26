using BG_Games.Card_Game_Core.Cards.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text  _title;
    [SerializeField] private TMP_Text  _mana;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private SpriteRenderer _imageSR;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _dropLayer;

    public Cards Cards { get; private set; }
    private Vector3 dragStartPosition;

    private Quaternion dragStartRotation;

     

    public void Setup(Cards cards)
    {
        Cards = cards;
        _title.text = cards.Title;
        _description.text = cards.Description;
        _imageSR.sprite = cards.Image;
        _mana.text = cards.Mana.ToString();
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        _wrapper.SetActive(false);
        Vector3 hoverPosition = new Vector3(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Cards, hoverPosition);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        _wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        if(Cards.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else{
            Interactions.Instance.PlayerIsDragging = true;
        _wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();
        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }
        
    }

    void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerIsDragging) return;
        if(Cards.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if(Cards.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if(target!=null && ManaSystem.Instance.HasEnoughMana(Cards.Mana))
            {
                PlayCardGA playCardGA = new(Cards, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else{
            if (ManaSystem.Instance.HasEnoughMana(Cards.Mana) 
        && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, _dropLayer))
        {
            PlayCardGA playCardGA = new (Cards);
            ActionSystem.Instance.Perform(playCardGA);
        }
        else
        {
            //return to hand
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }
        Interactions.Instance.PlayerIsDragging = false;
        }
        
    }
}
