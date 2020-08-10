using System;

namespace Match3.Features
{
    public class ColorComponentFeature : ICellObjectComponentFeature
    {
        public static readonly string Name = "Color"; 
        public static readonly ColorComponentFeature Instance = new ColorComponentFeature();

        public string FeatureId => Name;
        
        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IColorData colorData))
                throw new InvalidOperationException();
            
            return Construct(colorData);
        }

        public ICellObjectComponent Construct(IColorData data)
        {
            return new Color(data);
        }

        public class Color : ICellObjectComponent
        {
            public int ColorId { get; set; }
        
            //public Colored
            public string TypeId => Name;
            
            public Color(IColorData data)
            {
                ColorId = data.ColorId;
            }
        }

        public interface IColorData : ICellObjectComponentData
        {
            int ColorId { get; }
        }
    }
}