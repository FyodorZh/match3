using System;
using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public class ChipObjectFeature : IObjectFeature
    {
        public const string Name = "Chip";
        public static readonly ChipObjectFeature Instance = new ChipObjectFeature();

        public string FeatureId => Name;

        public IEnumerable<IObjectComponentFeature> DependsOn { get; } = new IObjectComponentFeature[]
        {
            ColorObjectComponentFeature.Instance,
            MassObjectComponentFeature.Instance,
            MoveObjectComponentFeature.Instance,
            HealthObjectComponentFeature.Instance,
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IChipData chipData))
                throw  new InvalidOperationException();

            return new Chip(chipData);
        }

        public interface IChip : ICellObject
        {
            ColorObjectComponentFeature.IColor Color { get; }
            int BodyType { get; }
        }

        public interface IChipData : ICellObjectData
        {
            ColorObjectComponentFeature.IColorData Color { get; }
            HealthObjectComponentFeature.IHealthData Health { get; }

            int BodyType { get; }
        }

        private class Chip : CellObject, IChip
        {
            public ColorObjectComponentFeature.IColor Color { get; }

            public int BodyType { get; }

            public Chip(IChipData data)
                : this(new ObjectTypeId(data.ObjectTypeId),
                    ColorObjectComponentFeature.Instance.Construct(data.Color),
                    MassObjectComponentFeature.Instance.Construct(),
                    MoveObjectComponentFeature.Instance.Construct(),
                    HealthObjectComponentFeature.Instance.Construct(data.Health))
            {
                BodyType = data.BodyType;
            }

            private Chip(
                ObjectTypeId typeId,
                ColorObjectComponentFeature.IColor color,
                MassObjectComponentFeature.IMass mass,
                MoveObjectComponentFeature.IMove move,
                HealthObjectComponentFeature.IHealth health)
                : base(typeId, color, mass, move, health)
            {
                Color = color;
            }
        }
    }
}