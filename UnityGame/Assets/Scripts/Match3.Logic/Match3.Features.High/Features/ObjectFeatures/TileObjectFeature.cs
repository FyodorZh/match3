using System;
using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public class TileObjectFeature : IObjectFeature
    {
        public const string Name = "Tile";
        public static readonly TileObjectFeature Instance = new TileObjectFeature();

        public string FeatureId => Name;

        public IEnumerable<IObjectComponentFeature> DependsOn { get; } = new IObjectComponentFeature[]
        {
            HealthObjectComponentFeature.Instance,
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is ITileData chipData))
                throw  new InvalidOperationException();

            return new Tile(chipData);
        }

        public interface ITile : ICellObject
        {
            HealthObjectComponentFeature.IHealth Health { get; }
        }

        public interface ITileData : ICellObjectData
        {
            int Health { get; }
        }

        private class Tile : CellObject, ITile
        {
            private class HealthData : HealthObjectComponentFeature.IHealthData
            {
                public string TypeId { get; }
                public int Priority { get; }
                public int HealthValue { get; }
                public DamageType Vulnerability { get; }
                public bool Fragile { get; }

                public HealthData(int healthValue)
                {
                    TypeId = HealthObjectComponentFeature.Name;
                    Priority = 5;
                    HealthValue = healthValue;
                    Vulnerability = DamageType.Match | DamageType.Explosion;
                    Fragile = true;
                }
            }

            public HealthObjectComponentFeature.IHealth Health { get; }
            public Tile(ITileData data)
                : this(new ObjectTypeId(data.ObjectTypeId),
                    HealthObjectComponentFeature.Instance.Construct(new HealthData(data.Health)))
            {
            }

            private Tile(
                ObjectTypeId typeId,
                HealthObjectComponentFeature.IHealth health)
                : base(typeId, health)
            {
                Health = health;
            }
        }
    }
}