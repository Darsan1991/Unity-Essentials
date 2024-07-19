using System;
using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public abstract class BaseQuery : AutoRegisterItem<IQueryItem>, IQueryItem
    {
        protected BaseQuery(string key, bool register = true, string localTag = null) : base(key, register, localTag)
        {
        }

        public object Ask(object obj)
        {
            if(Runner==null)
            {
                Debug.LogWarning("No Runner For Query:"+key);
                return default;
            }

            return Runner.Invoke(obj);
        }

        public Func<object, object> Runner { get; set; }
    }
}