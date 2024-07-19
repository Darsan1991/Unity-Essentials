using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public class EventEmitter<TJ> : BaseEventEmitter where TJ : IEventArgs
    {
        public EventEmitter(string key, Receiver<IProvider<string,IEvent>> receiver) : base(key,receiver)
        {
        }

        public void Raise(object caller, TJ args)
        {
            Item?.Raise(caller, args);
            if (Item == null)
                Debug.LogWarning("No Event Found:" + key);
        }
    }
    
    public class EventEmitter : BaseEventEmitter
    {
        public EventEmitter(string key, Receiver<IProvider<string,IEvent>> receiver) : base(key,receiver)
        {
        }

        public void Raise(object caller)
        {
            Item?.Raise(caller);
            if (Item == null)
                Debug.LogWarning("No Event Found:" + key);
        }
    }

    public class BaseEventEmitter:KeyBaseReceiverFromService<IEvent>
    {
        public BaseEventEmitter(string key, Receiver<IProvider<string,IEvent>> receiver) : base(key,receiver)
        {
        }
    }
}