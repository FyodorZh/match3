using System;

namespace Match3.Utils
{
    public interface IFactory<out T>
    {
        T Construct();
    }

    public interface IFactory<out T, in TParam1>
    {
        T Construct(TParam1 param1);
    }

    public class Factory<T> : IFactory<T>
    {
        private readonly Func<T> _factory;

        public Factory(Func<T> factory)
        {
            _factory = factory;
        }

        public T Construct()
        {
            return _factory();
        }
    }

    public class Factory<T, TParam1> : IFactory<T, TParam1>
    {
        private readonly Func<TParam1, T> _factory;

        public Factory(Func<TParam1, T> factory)
        {
            _factory = factory;
        }

        public T Construct(TParam1 param1)
        {
            return _factory(param1);
        }
    }

}