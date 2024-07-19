using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class Query<T,TJ> : BaseQuery,IQueryItem<T,TJ>
    {
        // ReSharper disable once TooManyDependencies
        public Query(string key,Func<T,TJ> runner, bool register = true, string localTag = null) : base(key, register, localTag)
        {
            Runner = o => runner( o is T arg ? arg : default );
        }


        public TJ Ask(T args)
        {
            return Ask(args as object) is TJ v ? v : default;
        }
    }

    public class Query<T> : BaseQuery, IQueryItem<T>
    {
        // ReSharper disable once TooManyDependencies
        public Query(string key, Func<T> runner, bool register = true, string localTag = null) : base(key, register,
            localTag)
        {
            Runner = _ => runner();
        }

        public T Ask()
        {
            var result = Ask(null);

            return Equals(result, default(T)) ? default : (T)result;
        }
    }

    public class Query : BaseQuery
    {
        // ReSharper disable once TooManyDependencies
        public Query(string key, Func<object, object> runner, bool register = true, string localTag = null) : base(key,
            register, localTag)
        {
            Runner = runner;
        }
    }
}