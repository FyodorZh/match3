using UnityEngine;

namespace Match3.View
{
    public class CellView : MonoBehaviour
    {
        private ICell _cell;
        private IGameRules _rules;

        public void Setup(ICell cell)
        {
            _cell = cell;
            _rules = cell.Game.Rules;

            foreach (var obj in cell.Content)
            {
                OnContentAdded(obj);
            }

            _cell.ContentAdded += OnContentAdded;
        }

        private void OnContentAdded(ICellObject obj)
        {
            var view = _rules.ViewFactory.Construct<CellObjectView>(obj);
            view.transform.SetParent(transform, false);
            view.name = obj.TypeId.Id;
        }
    }
}