using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class AutoRegisterItem<T> : IObjectItem, IDisposable
    {
        protected readonly string key;
        private IRepoProvider<string, T> _provider;

        public IRepoProvider<string, T> Provider
        {
            get => _provider;
            protected set
            {
                
                if (_provider == value)
                {
                    return;
                }
                
                _provider?.UnRegister(key);

                _provider = value;
                _provider?.Register(key,(T)(object)this);
            }
        }


        public AutoRegisterItem(string key,bool register = true,string localTag = null)
        {
            this.key = key;

            if(!register)
                return;
            
            ReceiverUtils.ReceiverToGetFromGlobalService<IRootRepoProvider<string,T>>().GetAndDisposeOnceReceived(p =>
            {
                Provider = (string.IsNullOrEmpty(localTag) ? p.GetLocal(localTag) : p);
            },this);
        }

        public void Dispose()
        {
            Provider = null;
        }
    }
    
 
}