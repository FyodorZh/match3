using System.Collections.Generic;
using Match3.Utils;

namespace Match3
{
    public interface ICell : ICellObserver
    {
        new IGame Game { get; }
        new IGrid Owner { get; }


        void AddLock(ILock lockObject);
        void RemoveLock(ILock lockObject);

        new IStaticSetView<ICellObject> Objects { get; }

        IReadOnlyList<ICellComponent> Components { get; }

        void AddComponent<TComponent>(TComponent component)
            where TComponent : class, ICellComponent, ICellComponentInitializer;

        bool CanAttach(ICellObject cellObject);
        void Attach(ICellObject cellObject);

        bool CanSwap(ICellObject cellObjectA, ICellObject cellObjectB);
        void Swap(ICellObject cellObjectA, ICellObject cellObjectB);

        bool Destroy(ICellObject cellObject);
    }
}