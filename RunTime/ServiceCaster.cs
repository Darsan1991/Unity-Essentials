using System;
using System.Collections.Generic;

namespace DGames.Essentials
{


    public class ServiceCaster<TJ> : IProvider<TypeAndTag, TJ>,IDisposable
    {
        private readonly IRepoProvider<TypeAndTag, object> _provider;

        public IEnumerable<IRepoProvider<TypeAndTag, TJ>> Subs => throw new Exception();
        public string Tag => _provider.Tag + "Caster";
        public event Action<TypeAndTag> Registered;
        public event Action<TypeAndTag> UnRegistered;
        public TJ Get(TypeAndTag typeAndTag, out bool success,bool allowSubs)
        {
            return (TJ)_provider.Get(typeAndTag, out success,allowSubs);
        }

       

        public void Register(TypeAndTag type, TJ instance)
        {
            throw new NotImplementedException();
        }

        public void UnRegister(TypeAndTag type, bool allowSubService = true)
        {
            throw new NotImplementedException();
        }

        public bool Has(TypeAndTag type, bool allowSubService = true)
        {
            return _provider.Has(type, allowSubService);
        }

        public void AddSub(IRepoProvider<TypeAndTag, TJ> service)
        {
            throw new NotImplementedException();
        }

        public void RemoveSub(IRepoProvider<TypeAndTag, TJ> service)
        {
            throw new NotImplementedException();
        }

        public ServiceCaster(IRepoProvider<TypeAndTag,object> provider)
        {
            _provider = provider;
            _provider.Registered += ProviderOnRegistered;
            _provider.UnRegistered += ProviderOnUnRegistered;
        }

        private void ProviderOnUnRegistered(TypeAndTag typeAndTag)
        {
            if (!typeof(TJ).IsAssignableFrom(typeAndTag.Type))
            {
                return;
            }

            UnRegistered?.Invoke(typeAndTag);
        }

        private void ProviderOnRegistered(TypeAndTag typeAndTag)
        {
            if (!typeof(TJ).IsAssignableFrom(typeAndTag.Type))
            {
                return;
            }

            Registered?.Invoke(typeAndTag);
        }

        public void Dispose()
        {
            _provider.Registered -= ProviderOnRegistered;
            _provider.UnRegistered -= ProviderOnUnRegistered;
        }
    }
}