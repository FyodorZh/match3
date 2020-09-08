namespace Match3.Features
{
    public abstract class CellComponentFeature : ICellComponentFeature
    {
        public abstract string FeatureId { get; }

        public abstract void InitState(IGame game);
    }
}