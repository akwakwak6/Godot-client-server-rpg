using Godot;
using System;

public class Network : Node
{
    private const int PORT = 8181;
    private const int NB_MAX_CLIENT = 20;
    private const string SERVER_IP = "127.0.0.1";

    private NetworkedMultiplayerENet NetworkPeer {get;set;}


    public override void _Ready()
    {
        NetworkPeer = new  NetworkedMultiplayerENet();
    }

    public void StartServer(){
        GD.Print("Start server");
        NetworkPeer.CreateServer(PORT,NB_MAX_CLIENT);
        GetTree().NetworkPeer = NetworkPeer;
        GetTree().Connect("network_peer_connected", this, "ServerPlayerConnected");
        GetTree().Connect("network_peer_disconnected", this, "ServerPlayerDisconnect");
    }

    public void StartClient(){
        GD.Print("Start client");
        NetworkPeer.CreateClient(SERVER_IP,PORT);
        GetTree().NetworkPeer = NetworkPeer;
        NetworkPeer.Connect("connection_succeeded", this, "ClientConnectedSucceeded");
        NetworkPeer.Connect("connection_failed", this, "ClientConnectedFaild");
    }

    private void ClientConnectedSucceeded(){
        GD.Print("connected success");
    }

    private void ClientConnectedFaild(){
        GD.Print("connected faild");
    }

    private void ServerPlayerConnected(int playerID){
        GD.Print($"Server player {playerID} connected");
    }

    private void ServerPlayerDisconnect(int playerID){
        GD.Print($"Server player {playerID} disconnect");
    }

}
















































