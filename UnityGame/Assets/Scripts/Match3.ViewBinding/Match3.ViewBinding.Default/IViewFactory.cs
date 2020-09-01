namespace Match3.ViewBinding.Default
{
    public interface IViewFactory
    {
        ObjectViewBinding Construct(IObjectObserver logicObject);
    }

    public static class IViewFactory_Ext
    {
        public static TObject Construct<TObject>(this IViewFactory factory, IObjectObserver logicObject)
            where TObject : ObjectViewBinding
        {
            var obj = factory.Construct(logicObject);
            if (obj is TObject typedObj)
            {
                return typedObj;
            }

            if (obj != null)
            {
                obj.Release();
            }
            return null;
        }
    }
}