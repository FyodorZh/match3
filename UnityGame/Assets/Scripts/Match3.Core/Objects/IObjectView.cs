namespace Match3
{
    public interface IObjectView 
    {
        IObject Owner { get; }
        void Release();
    }
}