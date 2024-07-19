using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public class CommandEmitter<T> : BaseCommandEmitter
    {
        public CommandEmitter(string key, Receiver<IProvider<string,ICommandItem>> receiver) : base(
            key, receiver)
        {
        }

        public void Emit(T args)
        {
            Item?.Execute(args);
            if (Item == null)
                Debug.LogWarning("Command Not Found:" + key);
        }
    }
    
    public class CommandEmitter : BaseCommandEmitter
    {
        public CommandEmitter(string key, Receiver<IProvider<string,ICommandItem>> receiver) : base(
            key,
            receiver)
        {
        }

        public void Emit()
        {
            Item?.Execute();
            if (Item == null)
                Debug.LogWarning("Command Not Found:" + key);
        }
    }

    public class BaseCommandEmitter:KeyBaseReceiverFromService<ICommandItem>
    {
        public BaseCommandEmitter(string key, Receiver<IProvider<string,ICommandItem>> receiver) : base(key, receiver)
        {
        }
    }
}