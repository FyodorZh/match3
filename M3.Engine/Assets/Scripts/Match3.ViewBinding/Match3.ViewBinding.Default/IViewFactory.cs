namespace Match3.ViewBinding.Default
{
    public interface IViewFactory
    {
        IObjectViewBinding Construct(ObjectTypeId typeId);
    }

    public static class IViewFactory_Ext
    {
        public static TObject Construct<TObject>(this IViewFactory factory, ObjectTypeId typeId)
            where TObject : class, IObjectViewBinding
        {
            var obj = factory.Construct(typeId);
            if (obj is TObject typedObj)
            {
                return typedObj;
            }

            obj?.Release();
            return null;
        }
    }
}