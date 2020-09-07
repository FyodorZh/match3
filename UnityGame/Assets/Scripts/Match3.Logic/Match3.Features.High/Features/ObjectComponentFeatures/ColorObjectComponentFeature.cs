using System;

namespace Match3.Features
{
    public class ColorObjectComponentFeature : ICellObjectComponentFeature
    {
        public const string Name = "Color";
        public static readonly ColorObjectComponentFeature Instance = new ColorObjectComponentFeature();

        public string FeatureId => Name;

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

        private class Color : CellObjectComponent, IColor
        {
            public int ColorId { get; }

            public override string TypeId => Name;

            public Color(IColorData data)
            {
                ColorId = data.ColorId;
            }
        }
    }
}