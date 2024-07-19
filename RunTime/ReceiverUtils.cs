namespace DGames.Essentials
{
    public static class ReceiverUtils
    {
        public static Receiver<T> ReceiverToGetFromGlobalService<T>()
        {
            return new Receiver<TypeAndTag, T>(
                new TypeAndTag { Type = typeof(T) },
                new ServiceCaster<T>(Services.GetService()),true);
        }
        
        public static Receiver<IProvider<T,TJ>> ProviderReceiverToGetFromGlobalService<T,TJ>()
        {
            return new Receiver<TypeAndTag, IProvider<T,TJ>>(
                new TypeAndTag { Type = typeof(IProvider<T,TJ>) },
                new ServiceCaster<IProvider<T,TJ>>(Services.GetService()),true);
        }
        public static Receiver<IRepoProvider<T,TJ>> RepoProviderReceiverToGetFromGlobalService<T,TJ>()
        {
            return new Receiver<TypeAndTag, IRepoProvider<T,TJ>>(
                new TypeAndTag { Type = typeof(IRepoProvider<T,TJ>) },
                new ServiceCaster<IRepoProvider<T,TJ>>(Services.GetService()),true);
        }
        
        public static Receiver<IProvider<T,TJ>> ProviderReceiverToGetFromLocalService<T,TJ>(string tag,object binding)
        {
            return ReceiverFromRepoProvider(RepoProviderReceiverToGetFromGlobalService<T, TJ>(), tag, binding);
        }
        
        public static Receiver<IProvider<T,TJ>> ReceiverFromRepoProvider<T,TJ>(Receiver<IRepoProvider<T,TJ>> repo, string tag,object obj)
        {
            var receiver = new ManualReceiver<IProvider<T,TJ>>();
            repo.GetAndDisposeOnceReceived(item =>
            {
                receiver.SetValue(item.GetSub(tag));
            },obj);
            return receiver;
        }
    }
}