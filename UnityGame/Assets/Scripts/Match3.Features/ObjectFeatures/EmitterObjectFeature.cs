using System;
using System.Collections.Generic;

namespace Match3.Features
{
    public class EmitterObjectFeature : IObjectFeature
    {
        public const string Name = "Emitter";

        public static readonly EmitterObjectFeature Instance = new EmitterObjectFeature();

        public string FeatureId => Name;

        public IEnumerable<IComponentFeature> DependsOn { get; } = new IComponentFeature[]
        {
            EmitterComponentFeature.Instance,
            MoveComponentFeature.Instance, 
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IEmitterObjectData emitterObjectData))
                throw new InvalidOperationException();
            
            return new CellObject(
                new ObjectTypeId(Name), 
                EmitterComponentFeature.Instance.Construct(emitterObjectData.Data));
        }

        public interface IEmitterObjectData : ICellObjectData
        {
            EmitterComponentFeature.IEmitterData Data { get; }
        }
    }
}