using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGames.Essentials
{
    public class AutoLocalProviderController<T,TJ>
    {
        private readonly IRootRepoProvider<T, TJ> _rootProvider;
        private readonly Func<string, IRepoProvider<T, TJ>> _creator;

        public AutoLocalProviderController(IRootRepoProvider<T,TJ> rootProvider,Func<string,IRepoProvider<T,TJ>> creator)
        {
            _rootProvider = rootProvider;
            _creator = creator;
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
        }

        public IRepoProvider<T, TJ> GetLocal(string sceneName, bool createIfNot = true)
        {
            if(createIfNot)
            {
                AddLocalServiceIfNotAlready(sceneName);
            }

            return _rootProvider.GetSub(sceneName);
        }
        
        private void SceneManagerOnSceneUnloaded(Scene scene)
        {
            RemoveLocalServiceIfHas(scene.name);
        }

        private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AddLocalServiceIfNotAlready(scene.name);
        }

        private  void AddLocalServiceIfNotAlready(string scene)
        {
            if (!_rootProvider.HasSub(scene))
            {
                _rootProvider.AddSub(_creator(scene));
                // Debug.Log($"Local Provider {_rootProvider} Added:"+scene);
            }
        }

        private void RemoveLocalServiceIfHas(string scene)
        {
            var service = GetLocal(scene);
            if (service != null)
            {
                _rootProvider.RemoveSub(service);
                // Debug.Log($"Local Provider {_rootProvider} Removed:"+scene);
            }
        }
    }
}