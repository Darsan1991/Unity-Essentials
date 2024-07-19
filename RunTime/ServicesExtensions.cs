using System;
using System.Collections.Generic;
using System.Linq;

namespace DGames.Essentials
{
    public static class ServicesExtensions
    {
        public static T GetOrDefault<T>(this ITypeBasedProvider provider, string tag = null, T def = default,bool allowSubService = true) =>
            provider.Has<T>(tag,allowSubService) ? provider.Get<T>(tag,allowSubService) : def;
        
        public static object GetOrDefault(this ITypeBasedProvider provider,Type type, string tag = null, object def = default,bool allowSubService = true) =>
            provider.Has(new TypeAndTag{Tag = tag,Type = type},allowSubService) ? provider.Get(new TypeAndTag{Tag = tag,Type = type},out _,allowSubService) : def;
        

        public static IEnumerable<T> GetAll<T>(this ITypeBasedProvider provider,bool allowSubServices = true)
        {
            return provider.GetAll(typeof(T), allowSubServices).Cast<T>();
        }

        public static void RegisterIfNotAlready<T,TJ>(this IRepoProvider<T,TJ> service, T key,TJ value)
        {
            if (service.Has(key))
                return;

            service.Register(key,value);
        }

        public static void RegisterIfNotAlready<T>(this ITypeBasedProvider provider, RegisterOptions options,
            string tag = null)
        {
            if (provider.Has<T>(tag))
            {
                return;
            }

            provider.Register<T>(options, tag);
        }
        
        public static void RegisterIfNotAlready(this ITypeBasedProvider provider,Type type, RegisterOptions options,
            string tag = null)
        {
            if (provider.Has(new TypeAndTag{Tag = tag,Type = type}))
            {
                return;
            }

            provider.Register(new TypeAndTag{Tag = tag,Type = type},options);
        }
        
        
        public static void Register<T>(this ITypeBasedProvider provider,T instance, string tag = null)
        {
            provider.Register(new TypeAndTag{Tag = tag,Type = typeof(T)},instance);
        }

        public static void UnRegister<T>(this ITypeBasedProvider provider,string tag = null, bool allowSubService = true)
        {
            provider.UnRegister(new TypeAndTag{Tag = tag,Type = typeof(T)},allowSubService);
        }
        
        public static void Register<T>(this ITypeBasedProvider provider,RegisterOptions options, string tag = null)
        {
            provider.Register(new TypeAndTag{Tag = tag,Type = typeof(T)},options);
        }
        
        
        public static T Get<T>(this ITypeBasedProvider provider,string tag = null, bool allowSubService = true)
        {
            return (T)provider.Get(new TypeAndTag{Tag = tag,Type = typeof(T)},out _,allowSubService);

        }
        

        public static bool Has<T>(this ITypeBasedProvider provider,string tag = null,bool allowSubService = true)
        {
            return provider.Has(new TypeAndTag{Tag = tag,Type = typeof(T)},allowSubService);
        }
    }
}