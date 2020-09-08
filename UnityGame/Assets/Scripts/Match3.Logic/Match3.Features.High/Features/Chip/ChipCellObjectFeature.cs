using System;
using Match3.Core;
using Match3.Features.Color;
using Match3.Features.Health;
using Match3.Features.Mass;
using Match3.Features.Move;

namespace Match3.Features.Chip
{
    public interface IChipCellObject : ICellObject, IChipCellObjectObserver
    {
        IColorCellObjectComponent Color { get; }
        int BodyType { get; }
    }

    public interface IChipCellObjectObserver : ICellObjectObserver
    {
        int ColorId { get; }
    }

    public interface IChipCellObjectData : ICellObjectData
    {
        IColorCellObjectComponentData Color { get; }
        IHealthCellObjectComponentData Health { get; }

        int BodyType { get; }
    }

    public class ChipCellObjectData : IChipCellObjectData
    {
        public string ObjectTypeId => ChipCellObjectFeature.Name;
        public IColorCellObjectComponentData Color { get; }
        public IHealthCellObjectComponentData Health { get; }

        public int BodyType => 0;

        public ChipCellObjectData(int colorId)
        {
            Color = new ColorCellObjectComponentData(colorId);
            Health = new HealthCellObjectComponentData(
                1,
                1,
                DamageType.Match | DamageType.Explosion,
                false);
        }
    }

    public abstract class ChipCellObjectFeature : CellObjectFeature
    {
        public const string Name = "Chip";
        public sealed override string FeatureId => Name;
    }

    namespace Default
    {
        public class ChipCellObjectFeatureImpl : ChipCellObjectFeature
        {
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
                if (!(data is IChipCellObjectData chipData))
                    throw new InvalidOperationException();

                return new ChipCellObject(chipData,
                    _colorComponentFeature.Construct(chipData.Color),
                    _massComponentFeature.Construct(),
                    _moveComponentFeature.Construct(),
                    _healthComponentFeature.Construct(chipData.Health));
            }

            private class ChipCellObject : CellObject, IChipCellObject
            {
                public IColorCellObjectComponent Color { get; }

                public int BodyType { get; }

                public int ColorId => Color.ColorId;

                public ChipCellObject(
                    IChipCellObjectData data,
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
}