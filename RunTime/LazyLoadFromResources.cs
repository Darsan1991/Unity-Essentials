using UnityEngine;

namespace DGames.Essentials
{
    public class LazyLoadFromResources<T> : LazyLoadValue<T> where T : Object
    {
        public LazyLoadFromResources(string path,T def = default) : base(()=>Resources.Load<T>(path), def)
        {
        }
    }
}