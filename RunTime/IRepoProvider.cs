using System;
using System.Collections.Generic;
using System.Linq;

namespace DGames.Essentials
{

    public interface IRootRepoProvider<T, TJ> : IRepoProvider<T, TJ>
    {
        IRepoProvider<T, TJ> GetLocal(string sceneName,bool createIfNot=true);
    }
    public interface IRepoProvider<T, TJ>:IProvider<T,TJ>
    {
        IEnumerable<IRepoProvider<T,TJ>> Subs { get; }
        string Tag { get; }
        void Register(T type, TJ instance);        
        void UnRegister(T type, bool allowSubService = true);
        
        
        void AddSub(IRepoProvider<T,TJ> service);
        void RemoveSub(IRepoProvider<T,TJ> service);

        IRepoProvider<T,TJ> GetSub(string tag) => Subs.FirstOrDefault(s => s.Tag == tag);

        bool HasSub(string tag) => Subs.Any(s => s.Tag == tag);
    }

    public interface IProvider<T, out TJ>
    {
        event Action<T> Registered;
        event Action<T> UnRegistered;
        TJ Get(T key,out bool success,bool allowSubService=true);
        bool Has(T type, bool allowSubService = true);
    }
}