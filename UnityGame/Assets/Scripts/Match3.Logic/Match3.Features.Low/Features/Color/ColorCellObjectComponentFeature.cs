namespace Match3.Features.Color
{
    public interface IColorCellObjectComponent : ICellObjectComponent
    {
        int ColorId { get; }
    }

    public interface IColorCellObjectComponentData : ICellObjectComponentData
    {
        int ColorId { get; }
    }

    public class ColorCellObjectComponentData : IColorCellObjectComponentData
    {
        public int ColorId { get; }

        public ColorCellObjectComponentData(int colorId)
        {
            ColorId = colorId;
        }
    }

    public abstract class ColorCellObjectComponentFeature : CellObjectComponentFeature
    {
        public const string Name = "Color";

        public sealed override string FeatureId => Name;

        public abstract IColorCellObjectComponent Construct(IColorCellObjectComponentData data);
    }

    namespace Default
    {
        public class ColorCellObjectComponentFeatureImpl : ColorCellObjectComponentFeature
        {
            public override IColorCellObjectComponent Construct(IColorCellObjectComponentData data)
            {
                return new Color(data);
            }

            private class Color : CellObjectComponent, IColorCellObjectComponent
            {
                public int ColorId { get; }

                public override string TypeId => Name;

                public Color(IColorCellObjectComponentData data)
                {
                    ColorId = data.ColorId;
                }

                public override ICellObjectComponentData SaveAsData()
                {
                    return new ColorCellObjectComponentData(ColorId);
                }
            }
        }
    }
}