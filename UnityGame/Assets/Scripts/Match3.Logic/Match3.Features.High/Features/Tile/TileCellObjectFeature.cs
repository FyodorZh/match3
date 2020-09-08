using System;
using Match3.Core;
using Match3.Features.Health;

namespace Match3.Features.Tile
{
    public interface ITileCellObject : ICellObject, ITileCellObjectObserver
    {
        //IHealthCellObjectComponent Health { get; }
    }

    public interface ITileCellObjectObserver : ICellObjectObserver
    {
        int Health { get; }
    }

    public interface ITileCellObjectData : ICellObjectData
    {
        int Health { get; }
    }

    public class TileCellObjectData : ITileCellObjectData
    {
        public string ObjectTypeId => TileCellObjectFeature.Name;

        public int Health { get; }

        public TileCellObjectData(int health)
        {
            Health = health;
        }
    }

    public abstract class TileCellObjectFeature : CellObjectFeature
    {
        public const string Name = "Tile";
        public sealed override string FeatureId => Name;
    }

    namespace Default
    {
        public class TileCellObjectFeatureImpl : TileCellObjectFeature
        {
            private HealthCellObjectComponentFeature _healthComponentFeature;

            public override void Init(IGameRules rules)
            {
                _healthComponentFeature = rules.GetCellObjectComponentFeature<HealthCellObjectComponentFeature>(HealthCellObjectComponentFeature.Name);
            }

            public override IObject Construct(IObjectData data)
            {
                if (!(data is ITileCellObjectData chipData))
                    throw new InvalidOperationException();

                return new TileCellObject(chipData,
                    _healthComponentFeature.Construct(new HealthCellObjectComponentData(
                        5,
                        chipData.Health,
                        DamageType.Match | DamageType.Explosion,
                        true)
                    ));
            }

            private class TileCellObject : CellObject, ITileCellObject
            {
                public IHealthCellObjectComponent Health { get; }

                int ITileCellObjectObserver.Health => Health.HealthValue;

                public TileCellObject(
                    ITileCellObjectData data,
                    IHealthCellObjectComponent health)
                    : base(new ObjectTypeId(data.ObjectTypeId), health)
                {
                    Health = health;
                }
            }
        }
    }
}