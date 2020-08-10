namespace Match3.Features
{
    public class ColorComponentFeature : ICellObjectComponentFeature
    {
        public static readonly string Name = "Color"; 
        public static readonly ColorComponentFeature Instance = new ColorComponentFeature();

        public string FeatureId => Name;
        
        public IObjectComponent Construct()
        {
            return TypedConstruct();
        }

        public ICellObjectComponent TypedConstruct()
        {
            return new Color();
        }

        public class Color : ICellObjectComponent
        {
            public int ColorId { get; set; }
        
            //public Colored
            public string TypeId => Name;
        }
    }
}