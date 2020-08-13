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

        bool CanAttach(ICellObject cellObject);
        bool Attach(ICellObject cellObject);

        bool Destroy(ICellObject cellObject);
    }

    public static class ICell_Ext
    {
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
    }
}