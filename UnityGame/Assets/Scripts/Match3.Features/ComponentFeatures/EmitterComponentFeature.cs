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
        }
    }
}