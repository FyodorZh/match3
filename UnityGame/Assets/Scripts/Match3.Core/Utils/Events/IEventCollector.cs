namespace Match3.Core
{
    public interface IEventCollector<in TEvent>
    {
        void Put(TEvent evt);
    }
}