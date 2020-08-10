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
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IChipData chipData))
                throw  new InvalidOperationException();
            
            return new CellObject(
                new ObjectTypeId(Name),
                ColorComponentFeature.Instance.Construct(chipData.Color));
        }
        
        public interface IChipData : ICellObjectData
        {
            ColorComponentFeature.IColorData Color { get; }
        }
    }
}