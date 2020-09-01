namespace Match3.Utils
{
    public interface IStaticSetView<TBase>
        where TBase : class
    {
        int Count { get; }

        TObject Get<TObject>()
            where TObject : TBase;

        StaticSetEnumerator<TBase> GetEnumerator();
    }
}