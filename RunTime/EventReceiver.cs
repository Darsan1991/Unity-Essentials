using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class EventReceiver<T> : BaseEventReceiver
    {
        public Binder<IEvent, T> ArgsBinder { get; } = new();

        public EventReceiver(string key, Receiver<IProvider<string,IEvent>> receiver) : base(key, receiver)
        {
        }

        protected override void OnItemBinderCalled(IEvent val)
        {
            base.OnItemBinderCalled(val);
            ArgsBinder.Raised(val, val.GetArgs<T>());
        }
    }
    
    public class EventReceiver : BaseEventReceiver
    {
        public Binder<IEvent> ArgsBinder { get; } = new();

        public EventReceiver(string key, Receiver<IProvider<string,IEvent>> receiver) : base(key,receiver)
        {
        }

        protected override void OnItemBinderCalled(IEvent val)
        {
            base.OnItemBinderCalled(val);
            ArgsBinder.Raised(val);
        }
    }

    public class BaseEventReceiver:KeyBasedContentReceiver< IEvent, IEvent>
    {
        public BaseEventReceiver(string key, Receiver<IProvider<string,IEvent>> receiver) : base(key,receiver)
        {
        }
    }

}