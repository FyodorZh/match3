namespace Match3
{
    public interface IObjectComponent
    {
        string TypeId { get; }
        bool IsReleased { get; }
        void Release();
    }
}