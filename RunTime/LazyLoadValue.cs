using System;

namespace DGames.Essentials
{
    public class LazyLoadValue<T>
    {
        private readonly Func<T> _getValue;
        private readonly T _def;
        private T _value;
        private bool _cache;

        public T Value
        {
            get
            {
                if (_cache && !Equals(_value, _def)) return _value;
                _value = _getValue();
                _cache = true;

                return _value;
            }
        }

        public LazyLoadValue(Func<T> getValue, T def = default)
        {
            _getValue = getValue;
            _def = def;
        }

        public static implicit operator T(LazyLoadValue<T> lazyLoadValue) => lazyLoadValue.Value;
    }
}