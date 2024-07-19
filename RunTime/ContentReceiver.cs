using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class ContentReceiver<TJ, T> : Receiver<TJ, T> where T : IBaseBinderProvider
    {
        public override T Item
        {
            get => base.Item;
            protected set
            {
                base.Item?.BaseBinder.UnBind(this);
                base.Item = value;
                base.Item?.BaseBinder.Bind(OnItemBinderCalled, this);
            }
        }

        public Binder<object> ContentBaseBinder { get; } = new();

        public ContentReceiver(TJ key, Receiver<IProvider<TJ,T>> receiver) :
            base(
                key,
                receiver
                )
        {
        }

        protected virtual void OnItemBinderCalled(object val)
        {
            ContentBaseBinder.Raised(val);
        }

        public override void Dispose()
        {
            base.Dispose();
            Item = default;
        }
    }

    public class KeyBasedContentReceiver<T, TJj> : ContentReceiver<string, T, TJj> where T : IBinderProvider<TJj>
    {
        public KeyBasedContentReceiver(string key, Receiver<IProvider<string,T>> receiver) : base(key,receiver)
        {
            
        }
    }

    public class KeyBasedContentReceiver<T> : ContentReceiver<string, T> where T : IBaseBinderProvider
    {
        public KeyBasedContentReceiver(string key, Receiver<IProvider<string,T>> receiver) : base(key, receiver)
        {
        }
    }


    public class ContentReceiver<TJ, T,TJj> : Receiver<TJ, T> where T : IBinderProvider<TJj>
    {
        public override T Item
        {
            get => base.Item;
            protected set
            {
                base.Item?.Binder.UnBind(this);
                base.Item = value;
                base.Item?.Binder.Bind(OnItemBinderCalled, this);
            }
        }

        public Binder<TJj> ContentBaseBinder { get; } = new();

        public ContentReceiver(TJ key, Receiver<IProvider<TJ,T>> receiver) :
            base(
                key,receiver)
        {
        }

        protected virtual void OnItemBinderCalled(TJj val)
        {
            ContentBaseBinder.Raised(val);
        }

        public override void Dispose()
        {
            base.Dispose();
            Item = default;
        }
    }
}