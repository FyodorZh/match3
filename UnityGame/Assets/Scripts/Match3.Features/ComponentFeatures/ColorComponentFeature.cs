using System;

namespace Match3.Features
{
    public class ColorComponentFeature : ICellObjectComponentFeature
    {
        public const string Name = "Color"; 
        public static readonly ColorComponentFeature Instance = new ColorComponentFeature();

        public string FeatureId => Name;
        
        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IColorData colorData))
                throw new InvalidOperationException();
            
            return Construct(colorData);
        }

        public IColor Construct(IColorData data)
        {
            return new Color(data);
        }
        
        public interface IColor : ICellObjectComponent
        {
            int ColorId { get; }
        }

        public interface IColorData : ICellObjectComponentData
        {
            int ColorId { get; }
        }
        
        private class Color : IColor
        {
            public int ColorId { get; }
        
            public string TypeId => Name;
            
            public Color(IColorData data)
            {
                ColorId = data.ColorId;
            }
        }
    }
}