using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class Event : AutoRegisterItem<IEvent>,IEvent
    {
        public event Action<IEvent> Raised;

        public Binder<object> BaseBinder { get; } = new();
        public Binder<IEvent> Binder { get; } = new();

        public IEventArgs ArgsValue { get; private set; }
        public object Caller { get; private set; }
        
        public void Raise(object caller = null, IEventArgs args = null)
        {
            Caller = caller;
            ArgsValue = args;
            BaseBinder.Raised(this);
            Binder.Raised(this);
            Raised?.Invoke(this);
        }


        public Event(string key, bool register = true, string localTag = null) : base(key, register, localTag)
        {
        }
    }
    
    
    public class Event<T> : Event,IEvent<T> where T : IEventArgs
    {
        public Event(string key, bool register = true, string localTag = null) : base(key, register, localTag)
        {
            
        }
    
        public void Raise(object caller = null, T args = default)
        {
            Raise(caller, Equals(args,default) ? default : (IEventArgs)args);
        }
    }
}