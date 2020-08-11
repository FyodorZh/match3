using System;
using Match3.Math;

namespace Match3.Features
{
    public class EmitterComponentFeature : ICellObjectComponentFeature
    {
        public static readonly string Name = "Emitter";
        public static readonly EmitterComponentFeature Instance = new EmitterComponentFeature();
        
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
            ICellObjectData ObjectToEmit { get; }
            int TimeOutMs { get; }
        }
        
        private class Emitter : CellObjectComponent, IEmitter
        {
            public override string TypeId => Name;

            private readonly ICellObjectData _objectDataToEmit;
            private readonly Fixed _timeout;

            private Fixed _timeTillSpawn;

            public Emitter(IEmitterData data)
            {
                _objectDataToEmit = data.ObjectToEmit;
                _timeout = new Fixed(data.TimeOutMs, 1000);
            }

            public ICellObject Emit(IGame game)
            {
                if (_timeTillSpawn > 0)
                {
                    return null;
                }

                _timeTillSpawn = _timeout;
                return game.Rules.ObjectFactory.Construct<ICellObject>(_objectDataToEmit, game);
            }

            public override void Tick(Fixed dTimeSeconds)
            {
                base.Tick(dTimeSeconds);
                _timeTillSpawn -= dTimeSeconds;
                if (_timeTillSpawn < 0)
                {
                    _timeTillSpawn = 0;
                }
            }
        }
    }
}