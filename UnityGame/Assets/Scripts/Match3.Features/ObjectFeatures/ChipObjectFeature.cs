using System.Collections.Generic;

namespace Match3.Features
{
    public class ChipObjectFeature : IObjectFeature
    {
        public static readonly string Name = "Chip";
        public static readonly ChipObjectFeature Instance = new ChipObjectFeature();
        
        public string FeatureId => Name;
        
        public IEnumerable<IComponentFeature> DependsOn { get; } = new IComponentFeature[]
        {
            ColorComponentFeature.Instance,
        };

        public IObject Construct()
        {
            return new CellObject(new ObjectTypeId(Name),
                ColorComponentFeature.Instance.TypedConstruct());
        }
    }
}