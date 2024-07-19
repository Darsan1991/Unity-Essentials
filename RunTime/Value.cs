using System;
using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public abstract class Value : AutoRegisterItem<IValue>,IValue
    {
        public Binder<object> BaseBinder { get; } = new();
        public event Action<IValue> Changed;
        public abstract object GetValue();

        public abstract void SetValue(object value);

        


        protected virtual void OnChangedEvent()
        {
            BaseBinder.Raised(GetValue());
            Changed?.Invoke(this);
        }

        protected Value(string key, bool register = true, string localTag = null) : base(key, register, localTag)
        {
        }

    
    }

    public class Value<T> : Value, IValue<T>
    {
        public Binder<T> Binder { get; } = new();
        public T CurrentValue { get; private set; }

        public static implicit operator T(Value<T> value) => value.CurrentValue;

        public override object GetValue() => Get();

        public T Get() => this;

        public void Set(T value)
        {
            CurrentValue = value;
            OnChangedEvent();
        }

        public override void SetValue(object value)
        {
            Set((T)value);
        }

        protected override void OnChangedEvent()
        {
            base.OnChangedEvent();
            Binder.Raised(this);
        }

        // ReSharper disable once TooManyDependencies
        public Value(string key, T defValue, bool register = true, string localTag = null) : base(key, register,
            localTag)
        {
            CurrentValue = defValue;
        }
    }
}