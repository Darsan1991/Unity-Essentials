using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DGames.Essentials
{
    public static class ScriptableUtils
    {
        private static readonly Dictionary<TypeAndTag, object> _cacheScriptables = new();

        public static T GetDefault<T>(string name = null)
        {
            var type = typeof(T);
            return GetDefault(type,name) is T t ? t : default;
        }
        
        public static object GetDefault(Type type,string name = null)
        {
            if (_cacheScriptables.ContainsKey(new TypeAndTag { Type = type, Tag = name }))
            {
                return _cacheScriptables[new TypeAndTag { Tag = name, Type = type }];
            }

            var path = "";
            var targetType = GetTargetType(type, name);
            if (targetType == null) return default;

            path = GetScriptablePathForScriptableType(targetType);

            // Debug.Log(targetType);
            // Debug.Log(path);
            // Debug.Log(Resources.Load(path, targetType));

            if (!typeof(ScriptableObject).IsAssignableFrom(targetType) || targetType.IsAbstract)
                return default;

            var result = (object)Resources.Load(path, targetType);
            _cacheScriptables.Add(new TypeAndTag { Tag = name, Type = type }, result);

            return result;
        }

        private static string GetScriptablePathForScriptableType(Type targetType)
        {
            var info = targetType.GetField("DEFAULT_FOLDER_PATH",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            return (info != null ? (string)info.GetValue(null) + "/" : "") + targetType.Name;
        }

        private static Type GetTargetType(Type type, string name)
        {
            Type targetType;
            if (!typeof(ScriptableObject).IsAssignableFrom(type) || type.IsAbstract)
            {
                targetType = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetTypes())
                    .SelectMany(t => t)
                    .FirstOrDefault(t =>
                        (string.IsNullOrEmpty(name) || name == t.Name) && !t.IsAbstract && type.IsAssignableFrom(t));
                // Debug.Log("Target Type:" + name);
            }
            else
            {
                targetType = type;
            }

            return targetType;
        }

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
    }
}