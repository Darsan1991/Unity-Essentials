using UnityEngine;

namespace DGames.Essentials
{
    public class LazyLoadFromComponent<T> : LazyLoadValue<T> where T : Component
    {
        public LazyLoadFromComponent(Component component, T def = default) : base(component.GetComponent<T>, def)
        {
        }
    }
}