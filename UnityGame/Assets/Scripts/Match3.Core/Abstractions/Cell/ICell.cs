using System.Collections.Generic;
using Match3.Core;

namespace Match3
{
    public interface ICell
    {
        CellId Id { get; }
        CellPosition Position { get; }

        IGame Game { get; }
        IGrid Owner { get; }

        bool IsActive { get; }

        bool IsLocked { get; }
        void AddLock(ILock lockObject);
        void RemoveLock(ILock lockObject);

        IReadOnlyList<ICellObject> Objects { get; }

        IReadOnlyList<ICellComponent> Components { get; }

        void AddComponent<TComponent>(TComponent component)
            where TComponent : class, ICellComponent, ICellComponentInitializer;

        bool CanAttach(ICellObject cellObject);
        bool Attach(ICellObject cellObject);

        bool Destroy(ICellObject cellObject);
    }

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
            var content = cell.Objects;
            int count = content.Count;
            for (int i = 0; i < count; ++i)
            {
                var component = content[i].TryGetComponent<TCellObjectComponent>();
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


        private static readonly CellNeighbours _cross = new CellNeighbours(new Offsets2D(new []
        {
            new[] {-1, 0},
            new[] {0, -1},
            new[] {1, 0},
            new[] {0, 1}
        }));

        private static readonly CellNeighbours _box = new CellNeighbours(new Offsets2D(new []
        {
            new[] {-1, -1},
            new[] {-1, 0},
            new[] {-1, 1},
            new[] {0, -1},
            new[] {0, 1},
            new[] {1, -1},
            new[] {1, 0},
            new[] {1, 1},
        }));

        public static IEnumerable<ICell> ActiveNeighboursInCross(this ICell cell)
        {
            return _cross.Enumerate(cell, true);
        }

        public static IEnumerable<ICell> ActiveNeighboursInBox(this ICell cell)
        {
            return _box.Enumerate(cell, true);
        }
    }
}