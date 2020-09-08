using System.Collections.Generic;
using Match3.Core;
using Match3.Features.Health;

namespace Match3.Features
{
    public interface IHealthCellComponent : ICellComponent, IHealthCellComponentObserver
    {
        void ApplyDamage(Damage damage);
    }

    public interface IHealthCellComponentObserver : ICellComponentObserver
    {
    }

    public abstract class HealthCellComponentFeature : CellComponentFeature
    {
        public static readonly string Name = "Health";
        public sealed override string FeatureId => Name;
    }

    namespace Default
    {
        public class HealthCellComponentFeatureImpl : HealthCellComponentFeature
        {
            public override void InitState(IGame game)
            {
                foreach (var grid in game.Board.Grids)
                {
                    foreach (var cell in grid.AllCells)
                    {
                        if (cell.IsActive)
                        {
                            cell.AddComponent(new HealthCellComponent());
                        }
                    }
                }
            }

            private class HealthCellComponent : CellComponent, IHealthCellComponent
            {
                private readonly List<IHealthCellObjectComponent> _components = new List<IHealthCellObjectComponent>();

                public override string TypeId => Name;

                public ICellObserver Owner => Cell;

                public void ApplyDamage(Damage damage)
                {
                    foreach (var obj in Cell.Objects)
                    {
                        var component = obj.TryGetComponent<IHealthCellObjectComponent>();
                        if (component != null)
                        {
                            _components.Add(component);
                        }
                    }

                    if (_components.Count > 0)
                    {
                        _components.Sort((l, r) => r.Priority.CompareTo(l.Priority));

                        int i = 0;
                        while (damage.Value > 0 && i < _components.Count)
                        {
                            var c = _components[i];
                            damage = c.ApplyDamage(damage);
                            ++i;
                        }

                        _components.Clear();
                    }
                }
            }
        }
    }
}