using Match3.Features.Move;

namespace Match3.Features.Emitter
{
    public interface IEmitterCellObjectComponent : ICellObjectComponent
    {
        ICellObject Emit(IGame game);
    }

    public interface IEmitterCellObjectComponentData : ICellObjectComponentData
    {
        ICellObjectData[] ObjectsToEmit { get; }
    }

    public class EmitterCellObjectComponentData : IEmitterCellObjectComponentData
    {
        public ICellObjectData[] ObjectsToEmit { get; }

        public EmitterCellObjectComponentData(ICellObjectData[] objectsToEmit)
        {
            ObjectsToEmit = objectsToEmit;
        }
    }

    public abstract class EmitterCellObjectComponentFeature : CellObjectComponentFeature
    {
        public const string Name = "Emitter";

        public sealed override string FeatureId => Name;

        public abstract IEmitterCellObjectComponent Construct(IEmitterCellObjectComponentData data);
    }

    namespace Default
    {
        public class EmitterCellObjectComponentFeatureImpl : EmitterCellObjectComponentFeature
        {
            public override IEmitterCellObjectComponent Construct(IEmitterCellObjectComponentData data)
            {
                return new Emitter(data);
            }

            private class Emitter : CellObjectComponent, IEmitterCellObjectComponent
            {
                public override string TypeId => Name;

                private readonly ICellObjectData[] _objectDataToEmit;

                private IMoveCellObjectComponent _prevObjectMoveComponent;

                public Emitter(IEmitterCellObjectComponentData data)
                {
                    _objectDataToEmit = data.ObjectsToEmit;
                }

                public ICellObject Emit(IGame game)
                {
                    if (_prevObjectMoveComponent != null && !_prevObjectMoveComponent.IsReleased)
                    {
                        var prevObject = _prevObjectMoveComponent.Owner;

                        var visiblePosY = prevObject.Owner.Position.Y + _prevObjectMoveComponent.Offset.Y;
                        if (Owner.Owner.Position.Y - visiblePosY < new Fixed(11, 10))
                        {
                            return null;
                        }
                    }

                    ICellObject obj = game.ObjectFactory.Construct<ICellObject>(_objectDataToEmit[game.GetRandom() % _objectDataToEmit.Length]);

                    var move = obj.TryGetComponent<IMoveCellObjectComponent>();
                    if (_prevObjectMoveComponent != null && !_prevObjectMoveComponent.IsReleased)
                    {
                        move.Velocity = new FixedVector2(0, -_prevObjectMoveComponent.Velocity.Length);
                    }

                    _prevObjectMoveComponent = move;

                    return obj;
                }

                protected override void OnRelease()
                {
                    base.OnRelease();
                    _prevObjectMoveComponent = null;
                }

                public override ICellObjectComponentData SaveAsData()
                {
                    return new EmitterCellObjectComponentData(_objectDataToEmit);
                }
            }
        }
    }
}