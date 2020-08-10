namespace Match3
{
    public interface IViewFactory
    {
        IObjectView Construct(IObject logicObject);
    }

    public static class IViewFactory_Ext
    {
        public static TObject Construct<TObject>(this IViewFactory factory, IObject logicObject)
            where TObject : class, IObjectView
        {
            var obj = factory.Construct(logicObject);
            if (obj is TObject typedObj)
            {
                return typedObj;
            }
            obj?.Release();
            return null;
        }
    }
}