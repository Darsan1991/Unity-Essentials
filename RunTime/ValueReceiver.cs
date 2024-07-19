using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class ValueReceiver<T> : ValueReceiver
    {
        public T CurrentValue => CurrentValueBase == null ? default : (T)CurrentValueBase;

        public Binder<T> ContentBinder { get; } = new();

        // ReSharper disable once TooManyDependencies
        public ValueReceiver(string key, T def, Receiver<IProvider<string,IValue>> receiver) : base(key, def, receiver)
        {
        }

        protected override void OnItemBinderCalled(object val)
        {
            base.OnItemBinderCalled(val);
            ContentBinder.Raised(CurrentValue);
        }

        public static implicit operator T(ValueReceiver<T> receiver) => receiver.CurrentValue;
    }

    public class ValueReceiver : KeyBasedContentReceiver<IValue>
    {
        private readonly object _def;
        public object CurrentValueBase => Item?.GetValue() ?? _def;

        public ValueReceiver(string key, object def, Receiver<IProvider<string,IValue>> receiver) : base(key, receiver)
        {
            _def = def;
        }
    }

}