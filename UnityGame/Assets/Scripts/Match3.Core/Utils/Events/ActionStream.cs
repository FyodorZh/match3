using System;

namespace Match3.Core
{
    public interface IActionCollector : IEventCollector<Action>
    {
        
    }
    
    public class ActionStream : EventStream<Action>, IActionCollector
    {
        public ActionStream()
            : base(a => a.Invoke())
        {
        }
    }
}