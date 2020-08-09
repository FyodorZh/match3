namespace Match3
{
    public interface IObject
    {
        IType Type { get; }
        void Release();
    }
}