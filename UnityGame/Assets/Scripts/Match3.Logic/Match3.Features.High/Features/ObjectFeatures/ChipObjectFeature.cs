using System;
using System.Collections.Generic;
using Match3.Core;
using Match3.Features.Color;
using Match3.Features.Health;
using Match3.Features.Mass;
using Match3.Features.Move;

namespace Match3.Features
{
    public class ChipObjectFeature : CellObjectFeature
    {
        public const string Name = "Chip";
        public static readonly ChipObjectFeature Instance = new ChipObjectFeature();

        public override string FeatureId => Name;

        private ColorCellObjectComponentFeature _colorComponentFeature;
        private MassCellObjectComponentFeature _massComponentFeature;
        private MoveCellObjectComponentFeature _moveComponentFeature;
        private HealthCellObjectComponentFeature _healthComponentFeature;

        public override void Init(IGameRules rules)
        {
            _colorComponentFeature = rules.GetCellObjectComponentFeature<ColorCellObjectComponentFeature>(ColorCellObjectComponentFeature.Name);
            _massComponentFeature = rules.GetCellObjectComponentFeature<MassCellObjectComponentFeature>(MassCellObjectComponentFeature.Name);
            _moveComponentFeature = rules.GetCellObjectComponentFeature<MoveCellObjectComponentFeature>(MoveCellObjectComponentFeature.Name);
            _healthComponentFeature = rules.GetCellObjectComponentFeature<HealthCellObjectComponentFeature>(HealthCellObjectComponentFeature.Name);
        }

        public override IObject Construct(IObjectData data)
        {
            if (!(data is IChipData chipData))
                throw  new InvalidOperationException();

            return new Chip(chipData,
                _colorComponentFeature.Construct(chipData.Color),
                _massComponentFeature.Construct(),
                _moveComponentFeature.Construct(),
                _healthComponentFeature.Construct(chipData.Health));
        }

        public interface IChip : ICellObject
        {
            IColorCellObjectComponent Color { get; }
            int BodyType { get; }
        }

        public interface IChipData : ICellObjectData
        {
            IColorCellObjectComponentData Color { get; }
            IHealthCellObjectComponentData Health { get; }

            int BodyType { get; }
        }

        private class Chip : CellObject, IChip
        {
            public IColorCellObjectComponent Color { get; }

            public int BodyType { get; }

            public Chip(
                IChipData data,
                IColorCellObjectComponent color,
                IMassCellObjectComponent mass,
                IMoveCellObjectComponent move,
                IHealthCellObjectComponent health)
                : base(new ObjectTypeId(data.ObjectTypeId), color, mass, move, health)
            {
                Color = color;
                BodyType = data.BodyType;
            }
        }
    }
}