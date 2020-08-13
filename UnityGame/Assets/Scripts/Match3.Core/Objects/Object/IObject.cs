using Match3.Core;

namespace Match3
{
    public interface IObject
    {
        ObjectTypeId TypeId { get; }

        void Release();
    }
}