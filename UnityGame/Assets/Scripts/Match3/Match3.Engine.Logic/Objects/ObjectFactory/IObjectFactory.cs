namespace Match3
{
    public interface IObjectFactory
    {
        IObject Construct(IObjectData data);
    }

    public static class IObjectFactory_Ext
    {
        public static TObject Construct<TObject>(this IObjectFactory factory, IObjectData data)
            where TObject : class, IObject
        {
            var obj = factory.Construct(data);
            if (obj is TObject typedObj)
            {
                return typedObj;
            }
            obj?.Release();
            return null;
        }
    }
}