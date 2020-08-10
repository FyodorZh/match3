using UnityEngine;

namespace Match3.View
{
    public class CellView : MonoBehaviour
    {
        private ICell _cell;

        public void Setup(ICell cell)
        {
            _cell = cell;

            IGame game = cell.Game;
            IGameRules rules = game.Rules;
            foreach (var obj in cell.Content)
            {
                var view = rules.ViewFactory.Construct<CellObjectView>(obj);
                view.transform.SetParent(transform, false);
                view.name = obj.TypeId.Id;
            }
        }
    }
}