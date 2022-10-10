using Godot;
using System;

public class Network : Node
{
    private const int PORT = 8181;
    private const int NB_MAX_CLIENT = 20;
    private const string SERVER_IP = "127.0.0.1";

    private NetworkedMultiplayerENet NetworkPeer {get;set;}
    private Boolean IsConnected = false;

    public readonly PackedScene PlayerSkinScene = GD.Load<PackedScene>("res://nodes/Player/PlayerSkin/PlayerSkin.tscn");

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
        //Rpc("RequestData","dataName",123);
        IsConnected = true;
    }

    private void ClientConnectedFaild(){
        GD.Print("connected faild");
        IsConnected = false;
    }

    private void ServerPlayerConnected(int playerID){
        GD.Print($"Server player {playerID} connected");
        KinematicBody ps = PlayerSkinScene.Instance<KinematicBody>();
        ps.Name = playerID.ToString();
        ps.SetNetworkMaster(playerID);
        AddChild(ps);
    }

    private void ServerPlayerDisconnect(int playerID){
        GD.Print($"Server player {playerID} disconnect");
    }

    [Remote]
    public void RequestData(string data,int requestNode){
        GD.Print($"send value of {data} => {requestNode}");
    }

    [Remote]
    public void PlayerData(Transform d){
        GD.Print("Update DATA");
        int pId = GetTree().GetRpcSenderId();
        GetNode<KinematicBody>(pId.ToString()).GlobalTransform = d;
        GD.Print(d);
    }

    public void SendPlayerData(Transform d){
        if( IsConnected )
            RpcId(1,"PlayerData",d);
    }

}




































