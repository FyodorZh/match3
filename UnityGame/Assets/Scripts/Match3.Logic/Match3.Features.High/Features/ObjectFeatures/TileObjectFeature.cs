using System;
using System.Collections.Generic;
using Match3.Core;
using Match3.Features.Health;

namespace Match3.Features
{
    public class TileObjectFeature : CellObjectFeature
    {
        public const string Name = "Tile";
        public static readonly TileObjectFeature Instance = new TileObjectFeature();

        public override string FeatureId => Name;

        private HealthCellObjectComponentFeature _healthComponentFeature;

        public override void Init(IGameRules rules)
        {
            _healthComponentFeature = rules.GetCellObjectComponentFeature<HealthCellObjectComponentFeature>(HealthCellObjectComponentFeature.Name);
        }

        public override IObject Construct(IObjectData data)
        {
            if (!(data is ITileData chipData))
                throw  new InvalidOperationException();

            return new Tile(chipData,
                _healthComponentFeature.Construct(new HealthCellObjectComponentData(
                    5,
                    chipData.Health,
                    DamageType.Match | DamageType.Explosion,
                    true)
                ));
        }

        public interface ITile : ICellObject
        {
            IHealthCellObjectComponent Health { get; }
        }

        public interface ITileData : ICellObjectData
        {
            int Health { get; }
        }

        private class Tile : CellObject, ITile
        {
            public IHealthCellObjectComponent Health { get; }

            public Tile(
                ITileData data,
                IHealthCellObjectComponent health)
                : base(new ObjectTypeId(data.ObjectTypeId), health)
            {
                Health = health;
            }
        }
    }
}