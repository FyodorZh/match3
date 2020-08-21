using System;
using System.Collections.Generic;
using System.Reflection;

namespace Match3.Utils
{
    class StaticSet<TBase> : IStaticSetView<TBase>
        where TBase : class
    {
        private static readonly Dictionary<Type, int> _typeMap = new Dictionary<Type, int>();
        private static readonly MethodInfo _getMethod = typeof(StaticSet<TBase>).GetMethod(nameof(GetId), BindingFlags.Static | BindingFlags.NonPublic);

        private readonly List<TBase> _list = new List<TBase>();

        private static int GetId<TObject>()
        {
            var id = TypeId<TObject>.Id;
            _typeMap.Add(typeof(TObject), id);
            return id;
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (var element in _list)
                {
                    if (element != null)
                        count += 1;
                }

                return count;
            }
        }

        public TObject Replace<TObject>(TObject newObject)
            where TObject : TBase
        {
            var id = GetId<TObject>();
            while (id >= _list.Count)
            {
                _list.Add(null);
            }

            TObject oldObject = (TObject)_list[id];
            _list[id] = newObject;
            return oldObject;
        }

        public TObject Get<TObject>()
            where TObject : TBase
        {
            var id = GetId<TObject>();
            while (id >= _list.Count)
            {
                _list.Add(null);
            }

            return (TObject)_list[id];
        }

        public StaticSetEnumerator<TBase> GetEnumerator()
        {
            return new StaticSetEnumerator<TBase>(_list);
        }

        public TBase Remove(Type type)
        {
            if (_typeMap.TryGetValue(type, out int id))
            {
                while (id >= _list.Count)
                {
                    _list.Add(null);
                }
                var o = _list[id];
                _list[id] = null;
                return o;
            }

            return null;
        }

        public TBase Replace(TBase newObject)
        {
            var type = newObject.GetType();
            if (!_typeMap.TryGetValue(type, out int id))
            {
                var method = _getMethod.MakeGenericMethod(type);
                id = (int)method.Invoke(this, new object[0]);
            }

            while (id >= _list.Count)
            {
                _list.Add(null);
            }

            var oldObject = _list[id];
            _list[id] = newObject;
            return oldObject;
        }

        private class TypeId
        {
            protected static int _id = 0;
        }

        private class TypeId<TObject> : TypeId
        {
            public static readonly int Id;

            static TypeId()
            {
                Id = System.Threading.Interlocked.Increment(ref _id) - 1;
            }
        }
    }
}