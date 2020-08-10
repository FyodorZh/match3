using System;

namespace Match3.Features
{
    public class EmitterComponentFeature : ICellObjectComponentFeature
    {
        private static readonly string Name = "Emitter";
        public static readonly EmitterComponentFeature Instance = new EmitterComponentFeature();
        
        public string FeatureId => Name;
        
        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IEmitterData typedData))
                throw new InvalidOperationException();
            
            return Construct(typedData);
        }

        public ICellObjectComponent Construct(IEmitterData data)
        {
            return new Emitter(data);
        }

        public class Emitter : ICellObjectComponent
        {
            public string TypeId => Name;

            private readonly ICellObjectData _objectDataToEmit;

            public Emitter(IEmitterData data)
            {
                _objectDataToEmit = data.ObjectToEmit;
            }

            public ICellObject Emit(IGame game)
            {
                return game.Rules.ObjectFactory.Construct<ICellObject>(_objectDataToEmit, game);
            }
        }
        
        public interface IEmitterData : ICellObjectComponentData
        {
            ICellObjectData ObjectToEmit { get; }
        }
    }
}