using System.Collections.Generic;

namespace Match3
{
    public static class ICell_Ext
    {
        public static TCellComponent FindComponent<TCellComponent>(this ICell cell)
            where TCellComponent : class, ICellComponent
        {
            var list = cell.Components;
            var count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                if (list[i] is TCellComponent typedComponent)
                {
                    return typedComponent;
                }
            }

            return null;
        }

        public static TCellObjectComponent FindObjectComponent<TCellObjectComponent>(this ICell cell)
            where TCellObjectComponent : class, ICellObjectComponent
        {
            foreach (var obj in cell.Objects)
            {
                var component = obj.TryGetComponent<TCellObjectComponent>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        public static ICell Under(this ICell cell)
        {
            var pos = cell.Position;
            pos = new CellPosition(pos.X, pos.Y - 1);
            return cell.Owner.GetCell(pos);
        }

        public static IEnumerable<ICell> ActiveNeighboursInCross(this ICell cell)
        {
            return CellNeighbours.PatternCross.Enumerate(cell, true);
        }

        public static IEnumerable<ICell> ActiveNeighboursInBox(this ICell cell)
        {
            return CellNeighbours.PatternBox.Enumerate(cell, true);
        }
    }
}