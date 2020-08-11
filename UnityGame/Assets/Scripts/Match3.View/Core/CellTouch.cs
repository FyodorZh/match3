using Match3;
using Match3.Features;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellTouch : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ICell _cell;
    
    public void Setup(ICell cell)
    {
        _cell = cell;
    }
    
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mass = _cell.FindComponent<MassComponentFeature.IMass>();
            if (mass != null)
            {
                mass.Owner.Release();
            }
        }
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var mass = _cell.FindComponent<MassComponentFeature.IMass>();
            if (mass != null)
            {
                var cell = mass.Owner.Owner;
                
                cell.Game.Action(KillActionFeature.Name, cell.Id);
            }
        }
    }

    private bool _inDrag;
    private Vector2 _dragStartPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _inDrag = true;
            _dragStartPos = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_inDrag)
        {
            var delta = eventData.position - _dragStartPos;
            Debug.Log("End" + delta);    
        }
    }
}
