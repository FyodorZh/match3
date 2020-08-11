using System;
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

            delta = delta.normalized;

            int dx = 0;
            int dy = 0;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                dx = delta.x > 0 ? 1 : -1;
            }
            else
            {
                dy = delta.y > 0 ? 1 : -1;
            }
            
            var pos1 = _cell.Position;
            var pos2 = new CellPosition(pos1.X + dx, pos1.Y + dy);

            var cell2 = _cell.Owner.GetCell(pos2);

            if (cell2 != null)
            {
                _cell.Game.Action(SwapActionFeature.Name, _cell.Id, cell2.Id);
            }
            
            Debug.Log("End " + delta + "   " + dx + ";" + dy);    
        }
    }
}
