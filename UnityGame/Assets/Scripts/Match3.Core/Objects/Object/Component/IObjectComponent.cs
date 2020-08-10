namespace Match3
{
    public interface IObjectComponent
    {
        string TypeId { get; }

        void Setup(IObjectComponentData data);
    }
}