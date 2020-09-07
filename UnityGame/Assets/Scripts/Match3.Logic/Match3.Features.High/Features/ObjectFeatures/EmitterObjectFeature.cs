using System;
using Match3.Core;
using Match3.Features.Emitter;

namespace Match3.Features
{
    public class EmitterObjectFeature : CellObjectFeature
    {
        public const string Name = "Emitter";

        public static readonly EmitterObjectFeature Instance = new EmitterObjectFeature();

        public override string FeatureId => Name;

        private EmitterCellObjectComponentFeature _emitterComponentFeature;

        public override void Init(IGameRules rules)
        {
            _emitterComponentFeature = rules.GetCellObjectComponentFeature<EmitterCellObjectComponentFeature>(EmitterCellObjectComponentFeature.Name);
        }

        public override IObject Construct(IObjectData data)
        {
            if (!(data is IEmitterObjectData emitterObjectData))
                throw new InvalidOperationException();

            return new Emitter(emitterObjectData, _emitterComponentFeature.Construct(emitterObjectData.Data));
        }

        public interface IEmitterObjectData : ICellObjectData
        {
            IEmitterCellObjectComponentData Data { get; }
        }

        public interface IEmitter : ICellObject
        {
        }

        private class Emitter : CellObject, IEmitter
        {
            public Emitter(IEmitterObjectData data, IEmitterCellObjectComponent emitter) :
                base(new ObjectTypeId(data.ObjectTypeId), emitter)
            {
            }

            public override bool CanAttachSibling(ICellObject sibling)
            {
                return false;
            }
        }
    }
}