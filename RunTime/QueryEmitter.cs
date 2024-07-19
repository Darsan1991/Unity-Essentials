using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public class QueryEmitter<T> : BaseQueryEmitter
    {
        public QueryEmitter(string key, Receiver<IProvider<string,IQueryItem>> receiver) : base(
            key,
            receiver)
        {
        }

        public T Ask()
        {
            if (Item != null) return Item.Ask<T, object>(null);
            Debug.LogWarning("Command Not Found:" + key);
            return default;
        }
    }
    
    public class QueryEmitter<T, TJ> : BaseQueryEmitter
    {
        public QueryEmitter(string key, Receiver<IProvider<string,IQueryItem>> receiver) : base(
            key,
            receiver)
        {
        }

        public TJ Ask(T args)
        {
            if (Item != null) return Item.Ask<TJ, T>(args);
            Debug.LogWarning("Command Not Found:" + key);
            return default;
        }
    }
    
    public class QueryEmitter : BaseQueryEmitter
    {
        public QueryEmitter(string key, Receiver<IProvider<string,IQueryItem>> receiver) : base(
            key,
            receiver)
        {
        }

        public object Ask()
        {
            if (Item == null)
                Debug.LogWarning("Command Not Found:" + key);

            return Item?.Ask(null);
        }
    }

    public abstract class BaseQueryEmitter : KeyBaseReceiverFromService<IQueryItem>
    {
        protected BaseQueryEmitter(string key, Receiver<IProvider<string,IQueryItem>> receiver) : base(key, receiver)
        {
        }
    }
}