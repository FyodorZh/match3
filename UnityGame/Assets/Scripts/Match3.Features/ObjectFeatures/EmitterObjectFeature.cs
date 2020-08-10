using System.Collections.Generic;

namespace Match3.Features
{
    public class EmitterObjectFeature : IObjectFeature
    {
        public static readonly string Name = "Emitter";
        
        public static readonly EmitterObjectFeature Instance = new EmitterObjectFeature();
        
        public string FeatureId => Name;
        
        public IEnumerable<IComponentFeature> DependsOn { get; } = new IComponentFeature[]
        {
            EmitterComponentFeature.Instance, 
        };

        public IObject Construct()
        {
            return new CellObject(new ObjectTypeId(Name), EmitterComponentFeature.Instance.TypedConstruct());
        }
    }
}