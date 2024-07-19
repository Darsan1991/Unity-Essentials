using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class Receiver<T,TJ>:Receiver<TJ>
    {
        protected readonly T key;
        private readonly bool _disposeProvider;
        private readonly bool _disposeProviderReceiver;
        private IProvider<T, TJ> _provider;
        private Receiver<IProvider<T, TJ>> _providerReceiver;
        
        private IProvider<T, TJ> Provider
        {
            get => _provider;
            set
            {
                if (Equals(_provider, value)) return;

                if (value != null)
                {
                    var item = value.Get(key,out var hasReceived);
                    HasReceived = hasReceived;
                    Item = hasReceived ? item : Item;
                }

                if (_provider != null)
                {
                    _provider.Registered -= ProviderOnRegistered;
                    _provider.UnRegistered -= ProviderOnUnRegistered;
                }

                _provider = value;
                
                if (_provider != null)
                {
                    _provider.Registered += ProviderOnRegistered;
                    _provider.UnRegistered += ProviderOnUnRegistered;
                }
            }
        }

        public Receiver<IProvider<T, TJ>> ProviderReceiver
        {
            get => _providerReceiver;
            private set
            {
                if (Equals(value,_providerReceiver))
                {
                    return;
                }
                
                

                _providerReceiver?.Binder.UnBind(this);
                _providerReceiver = value;
                if (_providerReceiver is { HasReceived: true }) Provider = _providerReceiver.Item; 
                _providerReceiver?.Binder.Bind(provider => Provider = provider,this);
            }
        }


        public Receiver(T key,IProvider<T,TJ> provider,bool disposeProvider = false)
        {
            this.key = key;
            _disposeProvider = disposeProvider;
            Provider = provider;
        }
        
        public Receiver(T key,Receiver<IProvider<T,TJ>> receiver,bool disposeProviderReceiver = true)
        {
            this.key = key;
            _disposeProviderReceiver = disposeProviderReceiver;
            ProviderReceiver = receiver;
        }


        private void ProviderOnUnRegistered(T key)
        {
            if(!Equals(key, this.key))
                return;
            HasReceived = false;
            Item = default;
        }

        private void ProviderOnRegistered(T key)
        {
            if(!Equals(key, this.key))
                return;
            HasReceived = true;
            Item = Provider.Get(key,out _);
        }

        public override void Dispose()
        {
            var providerReceiver = ProviderReceiver;
            ProviderReceiver = null;
            if(_disposeProviderReceiver)
            {
                providerReceiver?.Dispose();
            }

            var provider = Provider;
            Provider = null;

            if (_disposeProvider && provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    public static class ReceiverExtensions
    {
        public static void GetAndDisposeOnceReceived<T>(this Receiver<T> receiver, Action<T> onReceive,object binding)
        {
            if (receiver.HasReceived)
            {
                onReceive?.Invoke(receiver.Item);
                receiver.Dispose();
            }
            else
            {
                receiver.Binder.Bind(p =>
                {
                    receiver.Binder.UnBind(binding);
                    onReceive?.Invoke(p);
                    receiver.Dispose();
                },binding);
            }
        }
    }

    public abstract class Receiver<TJ> : IDisposable
    {
        private TJ _item;

        public virtual TJ Item
        {
            get => _item;
            protected set
            {
                if (Equals(_item, value))
                    return;
                _item = value;
                Binder.Raised(value);
            }
        }

        public bool HasReceived { get; protected set; }

        public Binder<TJ> Binder { get; } = new();

        public abstract void Dispose();
    }
    
    public class ManualReceiver<TJ> : Receiver<TJ>
    {
        public override void Dispose()
        {
            
        }

        public void SetValue(TJ t)
        {
            Item = t;
            HasReceived = true;
        }
    }
    
    public class FunReceiver<TJ> : Receiver<TJ>
    {
        private Action<TJ> _action;

        public override void Dispose()
        {
            _action -= OnCalled;
        }

        public FunReceiver(Action<TJ> action)
        {
            _action = action;
            _action += OnCalled;
        }

        private void OnCalled(TJ obj)
        {
            Item = obj;
            _action -= OnCalled;
        }
    }
}