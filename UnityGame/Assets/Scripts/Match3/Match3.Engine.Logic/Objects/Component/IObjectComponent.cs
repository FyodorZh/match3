namespace Match3
{
    public interface IObjectComponent : IObjectComponentObserver
    {
        void Release();
    }
}