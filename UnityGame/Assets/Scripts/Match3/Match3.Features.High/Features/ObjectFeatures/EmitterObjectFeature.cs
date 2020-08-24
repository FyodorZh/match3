using System;
using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public class EmitterObjectFeature : IObjectFeature
    {
        public const string Name = "Emitter";

        public static readonly EmitterObjectFeature Instance = new EmitterObjectFeature();

        public string FeatureId => Name;

        public IEnumerable<IObjectComponentFeature> DependsOn { get; } = new IObjectComponentFeature[]
        {
            EmitterObjectComponentFeature.Instance,
            MoveObjectComponentFeature.Instance,
        };

        public IObject Construct(IObjectData data)
        {
            if (!(data is IEmitterObjectData emitterObjectData))
                throw new InvalidOperationException();

            return new Emitter(emitterObjectData);
        }

        public interface IEmitterObjectData : ICellObjectData
        {
            EmitterObjectComponentFeature.IEmitterData Data { get; }
        }

        public interface IEmitter : ICellObject
        {
        }

        private class Emitter : CellObject, IEmitter
        {
            public Emitter(IEmitterObjectData data) :
                base(new ObjectTypeId(Name), EmitterObjectComponentFeature.Instance.Construct(data.Data))
            {
            }

            public override bool CanAttachSibling(ICellObject sibling)
            {
                return false;
            }
        }
    }
}