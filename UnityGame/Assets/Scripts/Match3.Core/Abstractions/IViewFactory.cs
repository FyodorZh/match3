namespace Match3
{
    public interface IViewFactory
    {
        IObjectView Construct(IType type, IGameContext context);
    }

    public static class IViewFactory_Ext
    {
        public static TObject Construct<TObject>(this IViewFactory factory, IType type, IGameContext context)
            where TObject : class, IObjectView
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