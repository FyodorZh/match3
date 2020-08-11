using System;
using System.Collections.Generic;

namespace Match3.Core
{
    public class EventStream<TEvent> : IEventCollector<TEvent>
    {
        private readonly List<TEvent>[] _events = { new List<TEvent>(), new List<TEvent>() };

        private readonly Action<TEvent> _onEvent;

        private int _listId = 0;

        public EventStream(Action<TEvent> onEvent)
        {
            _onEvent = onEvent;
        }

        public void Put(TEvent evt)
        {
            _events[_listId].Add(evt);
        }

        public void Process()
        {
            _listId = 1 - _listId; // 0, 1, 0, 1....
            
            foreach (var evt in _events[1 - _listId])
            {
                _onEvent.Invoke(evt);
            }
            _events[1 - _listId].Clear();
        }
    }
}