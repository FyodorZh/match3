using UnityEngine;

namespace Match3.View
{
    public class CellView : MonoBehaviour
    {
        private ICell _cell;

        public void Setup(ICell cell)
        {
            _cell = cell;
            
        }
    }
}