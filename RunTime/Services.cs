using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGames.Essentials
{
    public static class Services
    {
        public static event Action<TypeAndTag> Registered
        {
            add => _provider.Registered += value;
            remove => _provider.Registered -= value;
        }

        public static event Action<TypeAndTag> UnRegistered
        {
            add => _provider.UnRegistered += value;
            remove => _provider.UnRegistered -= value;
        }
        
        private static readonly IRootTypeBasedProvider _provider = new RootTypeBasedService("Global");

        public static IEnumerable<IRepoProvider<TypeAndTag, object>> SubServices => _provider.Subs;

    

        public static void Register<T>(T instance, string tag = null, GameObject go = null) =>
            (go != null ? GetLocalService(go.scene) : _provider).Register(new TypeAndTag { Type = typeof(T), Tag = tag },
                instance);


        public static void RegisterIfNotAlready<T>(T instance, string tag = null, GameObject go = null) =>
            (go != null ? GetLocalService(go.scene) : _provider).RegisterIfNotAlready(new TypeAndTag
            {
                Type = typeof(T),
                Tag = tag
            }, instance);
        
        public static void RegisterIfNotAlready<T,TJ,TJj>(T instance, string tag = null, GameObject go = null) where T:TJ,TJj
        {
            RegisterIfNotAlready<T>(instance, tag, go);
            RegisterIfNotAlready<TJ>(instance, tag, go);
            RegisterIfNotAlready<TJj>(instance, tag, go);
        }
        
        public static void RegisterIfNotAlready<T,TJ>(T instance, string tag = null, GameObject go = null) where T:TJ
        {
            RegisterIfNotAlready<T>(instance, tag, go);
            RegisterIfNotAlready<TJ>(instance, tag, go);
        }

        public static void Register<T>(RegisterOptions options, string tag = null, GameObject go = null) =>
            (go != null ? GetLocalService(go.scene) : _provider).Register(new TypeAndTag
            {
                Type = typeof(T),
                Tag = tag
            }, options);

        public static void RegisterIfNotAlready<T>(RegisterOptions options, string tag = null, GameObject go = null) =>
            (go != null ? GetLocalService(go.scene) : _provider).RegisterIfNotAlready<T>(options,tag);




        // public static void Register<T>(T instance, string tag = null,string subService=null) =>
        //     (string.IsNullOrEmpty(subService)
        //         ? _service
        //         : GetSubService(subService)).Register(instance, tag);
        //
        //
        // public static void RegisterIfNotAlready<T>(T instance, string tag = null,string subService=null) =>
        //     (string.IsNullOrEmpty(subService)
        //         ? _service
        //         : GetSubService(subService)).RegisterIfNotAlready(instance, tag);
        //
        // public static void Register<T>(RegisterOptions options, string tag = null,string subService=null) =>
        //     (string.IsNullOrEmpty(subService)
        //         ? _service
        //         : GetSubService(subService)).Register<T>(options, tag);
        //
        // public static void RegisterIfNotAlready<T>(RegisterOptions options, string tag = null,string subService=null) =>
        //     (string.IsNullOrEmpty(subService)
        //         ? _service
        //         : GetSubService(subService)).RegisterIfNotAlready<T>(options, tag);



        public static void UnRegister<T>(string tag = null, bool allowSubService = true)
        {
            _provider.UnRegister<T>(tag, allowSubService);
        }

        public static T GetOrDefault<T>(string tag = null, T def = default, bool allowSubService = true) =>
            _provider.GetOrDefault(tag, def, allowSubService);

        public static T Get<T>(string tag = null, bool allowSubService = true) => _provider.Get<T>(tag, allowSubService);

        public static bool Has<T>(string tag = null, bool allowSubService = true) =>
            _provider.Has<T>(tag, allowSubService);


        public static void Register(Type type, object instance, string tag = null, GameObject go = null) =>
            (go != null ? GetLocalService(go.scene) : _provider).Register(new TypeAndTag
            {
                Type = type,
                Tag = tag
            }, instance);



        public static void RegisterIfNotAlready(Type type, object instance, string tag = null, GameObject go = null) =>
            (go != null ? GetSubService(go.name) : _provider).RegisterIfNotAlready(new TypeAndTag
            {
                Type = type,
                Tag = tag
            }, instance);

        public static void Register(Type type, RegisterOptions options, string tag = null, GameObject go = null) =>
            (go != null ? GetSubService(go.name) : _provider).Register(new TypeAndTag
            {
                Type = type,
                Tag = tag
            }, options);

        public static void RegisterIfNotAlready(Type type, RegisterOptions options, string tag = null,
            GameObject go = null) =>
            (go != null ? GetSubService(go.name) : _provider).RegisterIfNotAlready(
                type, options,tag);

        public static void UnRegister(Type type, string tag = null, bool allowSubService = true) => _provider.UnRegister(
            new TypeAndTag
            {
                Type = type,
                Tag = tag
            }, allowSubService);

        public static object GetOrDefault(Type type, string tag = null, object def = default,
            bool allowSubService = true) => _provider.GetOrDefault(type, tag, def, allowSubService);

        public static object Get(Type type, string tag = null, bool allowSubService = true) => _provider.Get(
            new TypeAndTag
            {
                Type = type,
                Tag = tag
            }, out _, allowSubService);

        public static bool Has(Type type, string tag = null, bool allowSubService = true) => _provider.Has(new TypeAndTag
        {
            Type = type,
            Tag = tag
        }, allowSubService);

      

        public static void AddSubService(IRepoProvider<TypeAndTag, object> service) => _provider.AddSub(service);
        public static void RemoveSubService(IRepoProvider<TypeAndTag, object> service) => _provider.RemoveSub(service);
        public static IEnumerable<IRepoProvider<TypeAndTag, object>> GetAllServices() => _provider.Subs.Append(_provider);

        public static ITypeBasedProvider GetSubService(string tag) =>
            SubServices.Cast<ITypeBasedProvider>().FirstOrDefault(s => s.Tag == tag);

        public static IRepoProvider<TypeAndTag, object> GetService() => _provider;

        public static ITypeBasedProvider GetLocalService(Scene scene) => (ITypeBasedProvider)_provider.GetLocal(scene.name);
    }
}