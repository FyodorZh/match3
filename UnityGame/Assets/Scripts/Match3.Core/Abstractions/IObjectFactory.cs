namespace Match3
{
    public interface IObjectFactory
    {
        IObject Construct(IObjectType type, IGameContext context);
    }

    public static class IObjectFactory_Ext
    {
        public static TObject Construct<TObject>(this IObjectFactory factory, IObjectType type, IGameContext context)
            where TObject : class, IObject
        {
            var obj = factory.Construct(type, context);
            if (obj is TObject typedObj)
            {
                return typedObj;
            }
            obj?.Release();
            return null;
        }
    }
}