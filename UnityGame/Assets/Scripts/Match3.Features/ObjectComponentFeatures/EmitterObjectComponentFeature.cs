using System;
using Match3.Core;
using Match3.Math;

namespace Match3.Features
{
    public class EmitterObjectComponentFeature : ICellObjectObjectComponentFeature
    {
        public static readonly string Name = "Emitter";
        public static readonly EmitterObjectComponentFeature Instance = new EmitterObjectComponentFeature();

        public string FeatureId => Name;

        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IEmitterData typedData))
                throw new InvalidOperationException();

            return Construct(typedData);
        }

        public IEmitter Construct(IEmitterData data)
        {
            return new Emitter(data);
        }

        public interface IEmitter : ICellObjectComponent
        {
            ICellObject Emit(IGame game);
        }

        public interface IEmitterData : ICellObjectComponentData
        {
            ICellObjectData[] ObjectsToEmit { get; }
        }

        private class Emitter : CellObjectComponent, IEmitter
        {
            public override string TypeId => Name;

            private readonly ICellObjectData[] _objectDataToEmit;

            private MoveObjectComponentFeature.IMove _prevObjectMoveComponent;

            public Emitter(IEmitterData data)
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

                ICellObject obj = game.Rules.ObjectFactory.Construct<ICellObject>(_objectDataToEmit[game.GetRandom() % _objectDataToEmit.Length], game);

                var move = obj.TryGetComponent<MoveObjectComponentFeature.IMove>();
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
        }
    }
}