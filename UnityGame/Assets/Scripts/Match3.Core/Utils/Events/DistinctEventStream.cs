using System;
using System.Collections.Generic;

namespace Match3.Core
{
    public class DistinctEventStream<TEvent> : IEventCollector<TEvent>
        where TEvent : IEquatable<TEvent>
    {
        private readonly List<TEvent>[] _events = { new List<TEvent>(), new List<TEvent>() };
        private readonly HashSet<TEvent> _eventSet = new HashSet<TEvent>();

        private readonly Action<TEvent> _onEvent;

        private int _listId = 0;

        public DistinctEventStream(Action<TEvent> onEvent)
        {
            _onEvent = onEvent;
        }

        public void Put(TEvent evt)
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
                _onEvent.Invoke(evt);
            }
            _events[1 - _listId].Clear();
        }
    }
}