namespace DGames.Essentials
{
    public class PlayerPrefs : IPrefs
    {
        public  void SetInt(string key, int val) => UnityEngine.PlayerPrefs.SetInt(key, val);
        public  int GetInt(string key, int defVal = 0) => UnityEngine.PlayerPrefs.GetInt(key, defVal);
        public  void SetString(string key, string val) => UnityEngine.PlayerPrefs.SetString(key, val);
        public  string GetString(string key, string def = "") => UnityEngine.PlayerPrefs.GetString(key, def);
        public  void Clear() => UnityEngine.PlayerPrefs.DeleteAll();
        public  bool HasKey(string key) => UnityEngine.PlayerPrefs.HasKey(key);
        public  bool GetBool(string key, bool def = false) => GetInt(key, def ? 1 : 0) == 1;
        public  void SetBool(string key, bool val) => SetInt(key, val ? 1 : 0);
        public  void RemoveKey(string key) => UnityEngine.PlayerPrefs.DeleteKey(key);
    }
}