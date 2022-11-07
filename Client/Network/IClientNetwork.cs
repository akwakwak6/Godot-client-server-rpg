using System.Threading.Tasks;

public interface IClientNetwork{

    void StartClient();
    public void SendPlayerData(Player p);
    public Task<T> Request<T>(ConvertGodoData data)where T : ConvertGodoData,new();
    public Task<T> Request<T>()where T : ConvertGodoData,new();
}