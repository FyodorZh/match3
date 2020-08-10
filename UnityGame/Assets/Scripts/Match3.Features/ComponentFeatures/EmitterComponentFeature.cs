using System;

namespace Match3.Features
{
    public class EmitterComponentFeature : ICellObjectComponentFeature
    {
        private static readonly string Name = "Emitter";
        public static readonly EmitterComponentFeature Instance = new EmitterComponentFeature();
        
        public string FeatureId => Name;
        
        public IObjectComponent Construct()
        {
            return TypedConstruct();
        }

        public ICellObjectComponent TypedConstruct()
        {
            return new Emitter();
        }

        public class Emitter : ICellObjectComponent
        {
            public string TypeId => Name;

            private ICellObjectData _objectDataToEmit;
            
            public void Setup(IObjectComponentData data)
            {
                if (!(data is IEmitterData emitterData))
                    throw new InvalidOperationException();
                
                _objectDataToEmit = emitterData.ObjectToEmit;
            }

            public ICellObject Emit(IGame game)
            {
                return game.Rules.ObjectFactory.Construct<ICellObject>(_objectDataToEmit, game);
            }
        }
        
        public interface IEmitterData : IObjectComponentData
        {
            ICellObjectData ObjectToEmit { get; }
        }
    }
}