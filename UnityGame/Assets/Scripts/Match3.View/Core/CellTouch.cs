using Match3;
using Match3.Features;
using UnityEngine;

public class CellTouch : MonoBehaviour
{
    private ICell _cell;
    
    public void Setup(ICell cell)
    {
        _cell = cell;
    }
    
    private void OnMouseDown()
    {
        var mass = _cell.FindComponent<MassComponentFeature.IMass>();
        if (mass != null)
        {
            mass.Owner.Release();
        }
    }
}
