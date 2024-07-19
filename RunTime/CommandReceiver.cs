using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class CommandReceiver<T> : CommandReceiver
    {
        private readonly Action<ICommandItem, T> _action;

        public CommandReceiver(string key, Action<ICommandItem, T> action,
            Receiver<IProvider<string,ICommandItem>> receiver) :
            base(key, actionWithCommand: null, receiver)
        {
            _action = action;
        }
        
        public CommandReceiver(string key, Action<T> action, Receiver<IProvider<string,ICommandItem>> receiver) :
            base(key,actionWithCommand: null, receiver)
        {
            _action = (_, arg) =>  action(arg);
        }

        protected override void OnAction(ICommandItem item)
        {
            base.OnAction(item);
            _action?.Invoke(item, item.GetArgs<T>());
        }
    }
    
    public class CommandReceiver : KeyBaseReceiverFromService<ICommandItem>
    {
        public override ICommandItem Item
        {
            get => base.Item;
            protected set
            {
                if (base.Item != null) base.Item.Action = null;
                base.Item = value;
                if (value != null)
                {
                    if (value.Action != null)
                        throw new Exception();
                    value.Action = OnAction;
                }
            }
        }

        private readonly Action<ICommandItem> _action;

        public CommandReceiver(string key, Action<ICommandItem> actionWithCommand,
            Receiver<IProvider<string,ICommandItem>> receiver) : base(
            key,
            receiver)
        {
            _action = actionWithCommand;
        }
        
        public CommandReceiver(string key, Action action, Receiver<IProvider<string,ICommandItem>> receiver) : base(
            key,
            receiver)
        {
            _action = _ => action?.Invoke();
        }

        protected virtual void OnAction(ICommandItem item)
        {
            _action?.Invoke(item);
        }

        public override void Dispose()
        {
            base.Dispose();
            Item = null;
        }
    }
}