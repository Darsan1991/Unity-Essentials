namespace DGames.Essentials
{
    public interface IKeyBasedService<TJ>:IRepoProvider<string,TJ>
    {
        void UnRegister(TJ value,bool allowSubService = true);
        bool Has(TJ value,bool allowSubService = true);

        string GetKey(TJ value, bool allowSub = true);
    }
}