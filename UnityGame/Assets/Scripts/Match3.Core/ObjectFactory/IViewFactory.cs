namespace Match3
{
    public interface IViewFactory
    {
        IObjectView Construct(IObject logicObject, IGameContext context);
    }

    public static class IViewFactory_Ext
    {
        public static TObject Construct<TObject>(this IViewFactory factory, IObject logicObject, IGameContext context)
            where TObject : class, IObjectView
        {
            var obj = factory.Construct(logicObject, context);
            if (obj is TObject typedObj)
            {
                return typedObj;
            }
            obj?.Release();
            return null;
        }
    }
}