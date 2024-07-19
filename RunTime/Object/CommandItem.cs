using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public class CommandItem<T> : BaseCommandItem,ICommandItem<T>
    {
        // ReSharper disable once TooManyDependencies
        public CommandItem(string key, Action<ICommandItem,T> action,bool register = true, string localTag = null) : base(key, register, localTag)
        {
            Action = (item)=> action(item, ArgsValue is T v? v : default);
        }
        
        // ReSharper disable once TooManyDependencies
        public CommandItem(string key, Action<T> action,bool register = true, string localTag = null) : base(key, register, localTag)
        {
            Action = _=>action(ArgsValue is T v? v : default);
        }

    }

    public class CommandItem : BaseCommandItem
    {
        // ReSharper disable once TooManyDependencies
        public CommandItem(string key, Action<ICommandItem> action, bool register = true, string localTag = null) :
            base(key, register, localTag)
        {
            Action = action;
        }

        // ReSharper disable once TooManyDependencies
        public CommandItem(string key, Action action, bool register = true, string localTag = null) : base(key,
            register, localTag)
        {
            Action = _ => action();
        }
    }
}