using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public class ValueEmitter<TJ> : KeyBaseReceiverFromService<IValue>
    {
        private readonly TJ _def;

        // ReSharper disable once TooManyDependencies
        public ValueEmitter(string key, TJ def, Receiver<IProvider<string,IValue>> receiver) : base(key,
            receiver)
        {
            _def = def;
        }

        public void Set(TJ value)
        {
            if (Item != null)
            {
                Item.SetValue(value);
            }
            else
            {
                Debug.LogWarning("Value Not Found:" + key);
            }
        }

        public TJ CurrentValue => HasReceived ? (TJ)Item.GetValue() : _def;

        public static implicit operator TJ(ValueEmitter<TJ> emitter) => emitter.CurrentValue;
    }
}