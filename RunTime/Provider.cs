using System;
using System.Collections.Generic;
using System.Linq;
using DGames.Essentials.Extensions;

namespace DGames.Essentials
{
    public abstract class Provider<T,TJ> : IRepoProvider<T,TJ>
    {
        public event Action<T> Registered;
        public event Action<T> UnRegistered;

       
        private readonly List<IRepoProvider<T,TJ>> _subServices = new();
        

        public string Tag { get; }

        protected Provider(string tag)
        {
            Tag = tag;
        }
        
        

        public IEnumerable<IRepoProvider<T,TJ>> Subs => _subServices;

   

        public void Register(T key, TJ value)
        {
            ProcessRegister(key, value);
            RegisteredEvent(key);
        }

        protected void RegisteredEvent(T key)
        {
            Registered?.Invoke(key);
        }

        protected abstract void ProcessRegister(T key, TJ value);



        
        
        public void UnRegister(T key, bool allowSubService = true)
        {

            if (Has(key,false))
            {
                ProcessUnRegisterInThis(key);

                UnRegistered?.Invoke(key);
            }
            
            if(allowSubService)
                Subs.Where(s=> s.Has(key)).ForEach(s=>s.UnRegister(key));
        }

        protected abstract void ProcessUnRegisterInThis(T key);

        


        public abstract TJ Get(T key, out bool success, bool allowSubs = true);


        public abstract bool Has(T key, bool allowSubService = true);
        public void AddSub(IRepoProvider<T,TJ> service)
        {
            service.Registered += ServiceOnRegistered;
            service.UnRegistered += ServiceOnUnRegistered;
            _subServices.Add(service);
        }

        public void RemoveSub(IRepoProvider<T,TJ> service)
        {
            service.Registered -= ServiceOnRegistered;
            service.UnRegistered -= ServiceOnUnRegistered;
            _subServices.Remove(service);
        }

        private void ServiceOnUnRegistered(T obj)
        {
            Registered?.Invoke(obj);
        }

        private void ServiceOnRegistered(T obj)
        {
            UnRegistered?.Invoke(obj);
        }
    }
}