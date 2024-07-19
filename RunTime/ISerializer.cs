namespace DGames.Essentials
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T DeSerialize<T>(string str,out bool success);
    }
}