using Godot;
using System;

public partial class Network : IClientNetwork{
	private const string SERVER_IP = "127.0.0.1";
	private Boolean IsConnectedToServer = false;


	public void StartClient(){
		GD.Print("Start client");
		NetworkPeer = new NetworkedMultiplayerENet();
		NetworkPeer.CreateClient(SERVER_IP,PORT);
		this.GetTree().NetworkPeer = NetworkPeer;
		NetworkPeer.Connect("connection_succeeded", this,nameof(ConnectionSucceeded));
		NetworkPeer.Connect("connection_failed", this, nameof(ConnectionFaild));
	}

	private void ConnectionSucceeded(){
		GD.Print("connected success");
		//Rpc("RequestData","dataName",123);
		IsConnectedToServer = true;
	}

	private void ConnectionFaild(){
		GD.Print("connected faild");
		IsConnectedToServer = false;
	}
	
	public void SendPlayerData(Transform d){
		if( IsConnectedToServer )
			RpcId(1,"PlayerData",d);
	}
}




































