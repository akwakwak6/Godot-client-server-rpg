using Godot;

public interface IClientNetwork{

    void StartClient();
    void SendPlayerData(Transform d);
}