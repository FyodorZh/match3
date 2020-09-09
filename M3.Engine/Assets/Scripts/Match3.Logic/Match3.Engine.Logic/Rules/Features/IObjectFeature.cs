namespace Match3.Features
{
    public interface IObjectFeature : IFeature
    {
        void Init(IGameRules rules);

        IObject Construct(IObjectData data);
    }
}