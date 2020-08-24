namespace Match3.Utils
{
    public interface IEventCollector<in TEvent>
    {
        void Put(TEvent evt);
    }
}