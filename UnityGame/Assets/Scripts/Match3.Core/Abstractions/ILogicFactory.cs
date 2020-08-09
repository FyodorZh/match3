namespace Match3
{
    public interface ILogicFactory
    {
        ILogicObject Construct(IType type, IGameContext context);
    }

    public static class ILogicFactory_Ext
    {
        public static TObject Construct<TObject>(this ILogicFactory factory, IType type, IGameContext context)
            where TObject : class, ILogicObject
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