using System;
using System.Collections.Generic;
using DGames.Essentials.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


namespace DGames.Essentials
{
    public struct TypeAndTag
    {
        public Type Type { get; set; }
        public string Tag { get; set; }

        public bool Equals(TypeAndTag other)
        {
            return Type == other.Type && Tag == other.Tag;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeAndTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Tag);
        }
    }

    public interface ITypeBasedProvider :IRepoProvider<TypeAndTag,object>
    {
        IEnumerable<object> GetAll(Type type,bool allowSubService = true);
        void Register(TypeAndTag typeAndTag, RegisterOptions options);
    }
    
    public interface IRootTypeBasedProvider:ITypeBasedProvider,IRootRepoProvider<TypeAndTag,object>
    {
        
    }

    public static class ServiceProviderExtensions
    {
        public static T Get<T>(this IRepoProvider<TypeAndTag, object> provider, string tag, T def = default,bool allowSub = true)
        {
            var item = provider.Get(new TypeAndTag{Type = typeof(T),Tag = tag},out var success,allowSub);
            return success ? (T)item : def;
        }
    }

    public static class ProviderExtensions
    {
        public static IRepoProvider<T,TJ> GetLocal<T, TJ>(this IRepoProvider<T, TJ> provider, Scene scene)
        {
            return provider.GetSub(scene.name);
        }
        
        public static IRepoProvider<T,TJ> GetLocal<T, TJ>(this IRepoProvider<T, TJ> provider, string localTag)
        {
            return provider.GetSub(localTag);
        }
        
        
    }
    
    public class RegisterSettings
    {
        public List<RegisterSetting> Settings { get; } = new();

        public struct RegisterSetting
        {
            public string Tag { get; set; }
            public bool IsInstance { get; set; }
            public RegisterOptions Options { get; set; }
        }
    }

        public abstract class RegisterOptions
        {
            public bool IsNewInstanceEachTime { get; set; }
            public abstract object Get();
        }

        public class FuncBasedRegisterOptions<T> : RegisterOptions
        {
            private readonly Func<T> _func;

            public FuncBasedRegisterOptions(Func<T> func)
            {
                _func = func;
            }

            public override object Get() => _func();
        }


        public class TypeBasedRegisterOptions : RegisterOptions
        {
            public Type InstanceType { get; set; }

            public override object Get()
            {
                return Activator.CreateInstance(InstanceType);
            }
        }


        // ReSharper disable once ClassNeverInstantiated.Global
        public class UnityRegisterOptions : RegisterOptions
        {
            public FindType Find { get; set; }
            public string InstanceName { get; set; }

            public Type InstanceType { get; set; }

            public override object Get()
            {
                if (!(InstanceType.IsMonoBehavior() || InstanceType.IsScriptable()))
                {
                    throw new InvalidOperationException();
                }

                return Find switch
                {
                    FindType.AutoCreate => new GameObject(InstanceType.Name).AddComponent(InstanceType),
                    FindType.Resources => Resources.Load(InstanceName, InstanceType),
                    FindType.ResourcesPrefab => Object.Instantiate(Resources.Load(InstanceName, InstanceType)),
                    FindType.FindInScene => Object.FindObjectOfType(InstanceType),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            public enum FindType
            {
                AutoCreate,
                Resources,
                ResourcesPrefab,
                FindInScene
            }
        }

        


    
}