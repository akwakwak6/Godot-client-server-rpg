using Godot;

public interface IClientNetwork{

    void StartClient();
    void SendPlayerData(Player d);
}