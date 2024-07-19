using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DGames.Essentials
{
    public class JsonSerializer:ISerializer
    {
        public string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public T DeSerialize<T>(string str,out bool success)
        {
            if (string.IsNullOrEmpty(str))
            {
                success = false;
                return default;
            }

            success = true;
            return JsonUtility.FromJson<T>(str);
        }
    }

    public static class SerializerExtensions
    {
        public static T DeSerialize<T>(this ISerializer serializer, string str) =>
            serializer.DeSerialize<T>(str, out _);
    }
}

