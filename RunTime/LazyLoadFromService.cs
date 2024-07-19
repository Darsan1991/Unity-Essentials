using UnityEngine.SceneManagement;

namespace DGames.Essentials
{
    public class LazyLoadFromService<T> : LazyLoadValue<T>
    {
        public LazyLoadFromService(T def = default,string tag=null,bool isLocal = false,Scene scene = default) : base(() => isLocal ? Services.GetLocalService(scene).Get(tag,def) :  Services.GetOrDefault(def: def, tag: tag), def)
        {
        }
    }
}