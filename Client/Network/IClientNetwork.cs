using Godot;

public interface IClientNetwork{

    void StartClient();
    public void SendPlayerData(Player p);
}