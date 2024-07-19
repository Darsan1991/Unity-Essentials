namespace DGames.Essentials
{
    public class ReceiverFromService<TJ, T> : Receiver<TJ, T>
    {
        public ReceiverFromService(TJ key, Receiver<IProvider<TJ,T>> receiver) :
            base(
                key,receiver)
        {
        }
    }
    
    // public class ReceiverFromService<TJ, T> : Receiver<TJ, T>
    // {
    //     public ReceiverFromService(TJ key, bool local = false, string scene = null) :
    //         base(
    //             key,
    //             !local
    //                 ? ReceiverUtils.ProviderReceiverToGetFromGlobalService<TJ, T>()
    //                 : ReceiverUtils.ProviderReceiverToGetFromLocalService<TJ, T>(scene, new object()))
    //     {
    //     }
    // }

    public interface IProviderReceiverProvider
    {
        Receiver<TJ, T> GetGlobal<TJ, T>();
        Receiver<TJ, T> GetLocal<TJ, T>(string scene);
    }

    public class KeyBaseReceiverFromService<T>:Receiver<string, T>
    {
        public KeyBaseReceiverFromService(string key, Receiver<IProvider<string,T>> receiver) : base(key, receiver)
        {
        }
    }
}