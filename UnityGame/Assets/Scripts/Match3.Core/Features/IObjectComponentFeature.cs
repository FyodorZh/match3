namespace Match3
{
    public interface IObjectComponentFeature : IFeature
    {
        IObjectComponent Construct(IObjectComponentData data);
    }
}