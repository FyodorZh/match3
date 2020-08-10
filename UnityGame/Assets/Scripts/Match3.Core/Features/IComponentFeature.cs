namespace Match3
{
    public interface IComponentFeature : IFeature
    {
        IObjectComponent Construct(IObjectComponentData data);
    }
}