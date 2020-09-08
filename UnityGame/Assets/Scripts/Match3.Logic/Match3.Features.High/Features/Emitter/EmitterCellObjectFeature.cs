using System;
using Match3.Core;

namespace Match3.Features.Emitter
{
    public interface IEmitterCellObject : ICellObject, IEmitterCellObjectObserver
    {
    }

    public interface IEmitterCellObjectObserver : ICellObjectObserver
    {
    }

    public interface IEmitterCellObjectData : ICellObjectData
    {
        IEmitterCellObjectComponentData Data { get; }
    }

    public class EmitterCellObjectData : IEmitterCellObjectData
    {
        public string ObjectTypeId => EmitterCellObjectFeature.Name;
        public IEmitterCellObjectComponentData Data { get; }

        public EmitterCellObjectData(ICellObjectData[] objectsToEmit)
        {
            Data = new EmitterCellObjectComponentData(objectsToEmit);
        }
    }

    public abstract class EmitterCellObjectFeature : CellObjectFeature
    {
        public const string Name = "Emitter";
        public sealed override string FeatureId => Name;
    }

    namespace Default
    {
        public class EmitterCellObjectFeatureImpl : EmitterCellObjectFeature
        {
            private EmitterCellObjectComponentFeature _emitterComponentFeature;

            public override void Init(IGameRules rules)
            {
                _emitterComponentFeature = rules.GetCellObjectComponentFeature<EmitterCellObjectComponentFeature>(EmitterCellObjectComponentFeature.Name);
            }

            public override IObject Construct(IObjectData data)
            {
                if (!(data is IEmitterCellObjectData emitterObjectData))
                    throw new InvalidOperationException();

                return new EmitterCellObject(emitterObjectData, _emitterComponentFeature.Construct(emitterObjectData.Data));
            }

            private class EmitterCellObject : CellObject, IEmitterCellObject
            {
                public EmitterCellObject(IEmitterCellObjectData data, IEmitterCellObjectComponent emitter) :
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
}