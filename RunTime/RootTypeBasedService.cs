namespace DGames.Essentials
{
    public class RootTypeBasedService:TypeBasedService,IRootTypeBasedProvider
    {
        private readonly AutoLocalProviderController<TypeAndTag, object> _autoLocalProviderController;

        public RootTypeBasedService(string tag) : base(tag)
        {
            _autoLocalProviderController = new AutoLocalProviderController<TypeAndTag, object>(this,(scene)=> new TypeBasedService(scene));
        }

        public IRepoProvider<TypeAndTag, object> GetLocal(string sceneName, bool createIfNot = true)
        {
            return _autoLocalProviderController.GetLocal(sceneName, createIfNot);
        }
    }
}