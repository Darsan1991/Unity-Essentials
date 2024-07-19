namespace DGames.Essentials
{
    public static class PrefManager
    {
        public static IPrefs Prefs { get; set; } = new PlayerPrefs();
        public static void SetInt(string key, int val) => Prefs.SetInt(key, val);
        public static int GetInt(string key, int defVal = 0) => Prefs.GetInt(key, defVal);
        public static void SetString(string key, string val) => Prefs.SetString(key, val);
        public static string GetString(string key, string def = "") => Prefs.GetString(key, def);
        public static void Clear() => Prefs.Clear();
        public static bool HasKey(string key) => Prefs.HasKey(key);
        public static bool GetBool(string key, bool def = false) => GetInt(key, def ? 1 : 0) == 1;
        public static void SetBool(string key, bool val) => SetInt(key, val ? 1 : 0);
        public static void RemoveKey(string key) => Prefs.RemoveKey(key);
        public static void Set<T>(string key, T value) => Prefs.Set(key,value);
        public static T Get<T>(string key, T def) => Prefs.Get(key, def);

#if UNITY_EDITOR
        [UnityEditor.MenuItem("MyGames/Clear Prefs")]
        public static void ClearPres()
        {
            Prefs.Clear();
        }
#endif

    }
    
    
    
}