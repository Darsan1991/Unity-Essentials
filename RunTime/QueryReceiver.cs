using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{

    public abstract class BaseQueryReceiver : KeyBaseReceiverFromService<IQueryItem>
    {
        public override IQueryItem Item
        {
            get => base.Item;
            protected set
            {
                if (base.Item != null)
                    base.Item.Runner = null;
                base.Item = value;

                if (base.Item != null)
                    base.Item.Runner = Runner;
            }
        }

        protected BaseQueryReceiver(string key, Receiver<IProvider<string,IQueryItem>> receiver) : base(key, receiver)
        {
        }

        protected abstract object Runner(object obj);

        public override void Dispose()
        {
            base.Dispose();
            Item = null;
        }
    }
    
    public class QueryReceiver<T, TJ> : BaseQueryReceiver
    {
        private readonly Func<T, TJ> _runner;



        public QueryReceiver(string key, Func<T, TJ> runner, Receiver<IProvider<string,IQueryItem>> receiver) : base(key,receiver)
        {
            _runner = runner;
        }

       
        protected override object Runner(object obj)
        {
            return _runner(Equals(obj, default(T)) ? default : (T)obj);
        }
    }
    
    public class QueryReceiver<T> : BaseQueryReceiver
    {
        private readonly Func<T> _runner;
        
        public QueryReceiver(string key, Func<T> runner, Receiver<IProvider<string,IQueryItem>> receiver) : base(key, receiver)
        {
            _runner = runner;
        }

        protected override object Runner(object obj)
        {
            return _runner();
        }
    }
    
    public class QueryReceiver : QueryReceiver<object, object>
    {
        public QueryReceiver(string key, Func<object, object> runner, Receiver<IProvider<string,IQueryItem>> receiver) : base(
            key, runner, receiver)
        {
        }
    }
}