using System;

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
            
            public void Setup(IObjectComponentData data)
            {
                if (!(data is IColorData colorData))
                    throw new InvalidOperationException("Wrong data type");
                ColorId = colorData.ColorId;
            }
        }

        public interface IColorData : IObjectComponentData
        {
            int ColorId { get; }
        }
    }
}