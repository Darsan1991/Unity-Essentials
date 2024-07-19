using System;

namespace DGames.Essentials
{
    public interface IPrefs
    {
        void SetInt(string key, int val);
        int GetInt(string key, int defVal = 0);
        void SetString(string key, string val) ;
        string GetString(string key, string def = "");
        void Clear();
        bool HasKey(string key) ;
        bool GetBool(string key, bool def = false) ;
        void SetBool(string key, bool val);
        void RemoveKey(string key);
    }



}
