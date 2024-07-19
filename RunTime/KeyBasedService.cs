using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace DGames.Essentials
{
    public class KeyBasedService<T> : Provider<string, T>, IKeyBasedService<T>
    {
        private readonly Dictionary<string, T> _keyVsValues = new();

        public KeyBasedService(string tag) : base(tag)
        {
        }


        protected override void ProcessRegister(string key, T value)
        {
            if (_keyVsValues.ContainsKey(key))
            {
                throw new Exception();
            }

            _keyVsValues.Add(key, value);
        }

        protected override void ProcessUnRegisterInThis(string key)
        {
            _keyVsValues.Remove(key);
        }

        public void UnRegister(T value, bool allowSubService = true)
        {
            if (!Has(value, allowSubService)) return;
            
            var key = GetKey(value);
            if (!string.IsNullOrEmpty(key))
                UnRegister(key, allowSubService);
        }


        public bool Has(T value, bool allowSubService = true) => _keyVsValues.Values.Contains(value) ||
                                                                 allowSubService && Subs.Cast<IKeyBasedService<T>>()
                                                                     .Any(s => s.Has(value));

        public string GetKey(T value, bool allowSub = true) => _keyVsValues.ContainsValue(value)
            ? _keyVsValues.First(p => Equals(p.Value, value)).Key
            : allowSub
                ? Subs.Cast<IKeyBasedService<T>>().FirstOrDefault(s => s.Has(value))?.GetKey(value)
                : null;

        public override bool Has(string key, bool allowSubService = true)
        {
            return _keyVsValues.ContainsKey(key) || allowSubService && Subs.Any(s => s.Has(key));
        }


        public override T Get(string key, out bool success, bool allowSubs = true)
        {
            success = Has(key, allowSubs);
            if (Has(key, false))
            {
                return _keyVsValues[key];
            }

            return !allowSubs
                ? default
                : Subs.Where(s => s.Has(key)).Select(s => s.Get(key, out _)).FirstOrDefault();
        }
    }

    public class RootKeyBasedService<T> : KeyBasedService<T>,IRootRepoProvider<string,T>
    {
        private readonly AutoLocalProviderController<string, T> _autoLocalProviderController;

        public RootKeyBasedService(string tag) : base(tag)
        {
            _autoLocalProviderController = new AutoLocalProviderController<string, T>(this,(scene)=> new KeyBasedService<T>(scene));
        }

        public IRepoProvider<string, T> GetLocal(string sceneName, bool createIfNot = true)
        {
            return _autoLocalProviderController.GetLocal(sceneName, createIfNot);
        }
    }
}