using System.Collections;
using System.Collections.Generic;

namespace Match3.Utils
{
    public struct StaticSetEnumerator<TBase> : IEnumerator<TBase>
        where TBase : class
    {
        private readonly List<TBase> _list;
        private int _index;
        private TBase _current;

        public StaticSetEnumerator(List<TBase> list)
        {
            _list = list;
            _index = 0;
            _current = null;
        }

        public bool MoveNext()
        {
            while (_index < _list.Count && _list[_index] == null)
            {
                ++_index;
            }

            if (_index >= _list.Count)
            {
                _current = null;
                return false;
            }

            _current = _list[_index++];
            return true;
        }

        public TBase Current => _current;

        object IEnumerator.Current => _current;

        void IEnumerator.Reset()
        {
            _index = 0;
            _current = null;
        }

        public void Dispose()
        {
        }
    }
}