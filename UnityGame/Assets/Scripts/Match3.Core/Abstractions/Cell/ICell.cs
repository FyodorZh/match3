using System.Collections.Generic;

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
        void AddLock(object lockObject);
        void RemoveLock(object lockObject);
        
        IReadOnlyList<ICellObject> Objects { get; }

        bool CanAttach(ICellObject cellObject);
        bool Attach(ICellObject cellObject);

        bool Destroy(ICellObject cellObject);
    }

    public static class ICell_Ext
    {
        public static TCellObjectComponent FindComponent<TCellObjectComponent>(this ICell cell)
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

        public static (ICellObject, TCellObjectComponent) FindObjectWithComponent<TCellObjectComponent>(this ICell cell)
            where TCellObjectComponent : class, ICellObjectComponent
        {
            var content = cell.Objects;
            int count = content.Count;
            for (int i = 0; i < count; ++i)
            {
                var component = content[i].TryGetComponent<TCellObjectComponent>();
                if (component != null)
                {
                    return (content[i], component);
                }
            }

            return (null, null);
        }
    }
}