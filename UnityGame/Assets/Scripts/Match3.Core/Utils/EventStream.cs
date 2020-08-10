using System;
using System.Collections.Generic;

namespace Match3.Core
{
    public interface IEventStream<out TEvent>
    {
        event Action<TEvent> Event;
    }
    
    public class EventStream<TEvent> : IEventStream<TEvent>
        where TEvent : IEquatable<TEvent>
    {
        public event Action<TEvent> Event;
        
        private readonly List<TEvent>[] _events = { new List<TEvent>(), new List<TEvent>() };
        private readonly HashSet<TEvent> _eventSet = new HashSet<TEvent>();

        private int _listId = 0;

        public void FireEvent(TEvent evt)
        {
            if (_eventSet.Add(evt))
            {
               _events[_listId].Add(evt);
            }
        }

        public void Process()
        {
            _eventSet.Clear();
            _listId = 1 - _listId; // 0, 1, 0, 1....
            
            foreach (var evt in _events[1 - _listId])
            {
                Event?.Invoke(evt);
            }
            _events[1 - _listId].Clear();
        }
    }
}