using Match3;
using Match3.Features;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellTouch : MonoBehaviour, IPointerClickHandler
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
            // var mass = _cell.FindComponent<MassComponentFeature.IMass>();
            // if (mass != null)
            // {
            //     mass.Owner.Release();
            // }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var mass = _cell.FindComponent<MassComponentFeature.IMass>();
            if (mass != null)
            {
                mass.Owner.Release();
            }
        }
    }
}
