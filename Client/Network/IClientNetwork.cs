using System.Threading.Tasks;

public interface IClientNetwork{

    void StartClient();
    public void SendPlayerData(Player p);
    public Task<T> Request<T>(int cmd,ConvertGodoData data)where T : ConvertGodoData,new();
}