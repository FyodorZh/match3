using System;
using System.Collections.Generic;

namespace Match3.Features
{
    public class ChipObjectFeature : IObjectFeature
    {
        public const string Name = "Chip";
        public static readonly ChipObjectFeature Instance = new ChipObjectFeature();
        
        public string FeatureId => Name;
        
        public IEnumerable<IComponentFeature> DependsOn { get; } = new IComponentFeature[]
        {
            ColorComponentFeature.Instance,
            MassComponentFeature.Instance, 
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IChipData chipData))
                throw  new InvalidOperationException();

            return new Chip(chipData);
        }
        
        public interface IChip : ICellObject
        {
            ColorComponentFeature.IColor Color { get; }
            int BodyType { get; }
        }
        
        public interface IChipData : ICellObjectData
        {
            ColorComponentFeature.IColorData Color { get; }
            MoveComponentFeature.IMoveData Movement { get; }
            MassComponentFeature.IMassData Mass { get; }
            
            int BodyType { get; }
        }

        private class Chip : CellObject, IChip
        {
            public ColorComponentFeature.IColor Color { get; }
            
            public int BodyType { get; }

            public Chip(IChipData data)
                : this(new ObjectTypeId(data.TypeId), 
                    ColorComponentFeature.Instance.Construct(data.Color),
                    MassComponentFeature.Instance.Construct(data.Mass),
                    MoveComponentFeature.Instance.Construct(data.Movement))
            {
                BodyType = data.BodyType;
            }

            private Chip(
                ObjectTypeId typeId, 
                ColorComponentFeature.IColor color,
                MassComponentFeature.IMass mass,
                MoveComponentFeature.IMove move)
                : base(typeId, color, mass, move)
            {
                Color = color;
            }
        }
    }
}