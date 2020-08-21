using System;
using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public class HealthCellComponentFeature : ICellComponentFeature
    {
        public static readonly string Name = "Health";
        public static readonly HealthCellComponentFeature Instance = new HealthCellComponentFeature();

        public string FeatureId => Name;

        public IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; } = new ICellComponentFeature[]
        {
        };

        public IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
            HealthObjectComponentFeature.Instance,
        };

        public void InitState(IGame game)
        {
            foreach (var grid in game.Board.Grids)
            {
                foreach (var cell in grid.AllCells)
                {
                    if (cell.IsActive)
                    {
                        cell.AddComponent(new Health());
                    }
                }
            }
        }

        public interface IHealth : ICellComponent
        {
            void ApplyDamage(Damage damage);
        }

        private class Health : CellComponent, IHealth
        {
            private readonly List<HealthObjectComponentFeature.IHealth> _components = new List<HealthObjectComponentFeature.IHealth>();

            public override string TypeId => Name;

            public void ApplyDamage(Damage damage)
            {
                foreach (var obj in Cell.Objects)
                {
                    var component = obj.TryGetComponent<HealthObjectComponentFeature.IHealth>();
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