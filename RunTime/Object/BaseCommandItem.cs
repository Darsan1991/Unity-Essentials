using System;
using DGames.ObjectEssentials;

namespace DGames.Essentials
{
    public abstract class BaseCommandItem : AutoRegisterItem<ICommandItem>, ICommandItem
    {
        protected BaseCommandItem(string key, bool register = true, string localTag = null) : base(key, register, localTag)
        {
        }
        
        public void Execute()
        {
            
        }


        public object ArgsValue { get; set; }
        public Action<ICommandItem> Action { get; set; }
    }
}