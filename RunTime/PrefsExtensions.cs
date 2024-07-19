namespace DGames.Essentials
{
    public static class PrefsExtensions
    {
        public static void Set<T>(this IPrefs prefs, string key, T value,ISerializer serializer = null)
        {
            var type = typeof(T);
            if (type.IsPrimitive)
            {
                SetValueForPrimitive(prefs, key, value);
            }
            else
            {
                serializer ??= new JsonSerializer();
                prefs.SetString(key, serializer.Serialize(value));
            }

        }

        private static void SetValueForPrimitive<T>(IPrefs prefs, string key, T value)
        {
            var type = typeof(T);

            if (type == typeof(int))
            {
                prefs.SetInt(key, (int)(object)value);
            }
            else if (type == typeof(string))
            {
                prefs.SetString(key, (string)(object)value);
            }
            else if (type == typeof(bool))
            {
                prefs.SetBool(key, (bool)(object)value);
            }

        }

        public static T Get<T>(this IPrefs prefs, string key, T defValue,ISerializer serializer=null)
        {
            var type = typeof(T);
            if (type.IsPrimitive)
            {
                return (T)GetValueForPrimitive(prefs, key, defValue);
            }

            serializer ??= new JsonSerializer();
            var value = serializer.DeSerialize<T>(prefs.GetString(key),out var success);

            return success ? value : defValue;
        }

        private static object GetValueForPrimitive<T>(IPrefs prefs, string key, T defValue)
        {
            var type = typeof(T);

            object value = defValue;
            if (type == typeof(int))
            {
                value = prefs.GetInt(key, (int)(object)defValue);
            }
            else if (type == typeof(string))
            {
                value = prefs.GetString(key, (string)(object)defValue);
            }
            else if (type == typeof(bool))
            {
                value = prefs.GetBool(key, (bool)(object)defValue);
            }

            return value;
        }
    }
}