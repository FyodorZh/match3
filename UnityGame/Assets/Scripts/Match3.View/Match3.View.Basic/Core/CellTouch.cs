using Match3;
using Match3.Features;
using Match3.Features.Mass;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = Match3.Debug;

public class CellTouch : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ICellObserver _cell;
    private IGameController _controller;

    public void Setup(ICellObserver cell, IGameController controller)
    {
        _cell = cell;
        _controller = controller;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mass = _cell.FindObjectComponent<IMassCellObjectComponent>();
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
            var mass = _cell.FindObjectComponent<IMassCellObjectComponent>();
            if (mass != null)
            {
                var cell = mass.Owner.Owner;

                _controller.Action(KillActionFeature.Name, cell.Position);
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
                _controller.Action(SwapActionFeature.Name, _cell.Position, cell2.Position);
            }

            Debug.Log("End " + delta + "   " + dx + ";" + dy);
        }
    }
}
