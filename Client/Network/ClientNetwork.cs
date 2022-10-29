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
	
	public void SendPlayerData(Player p){
		
		if( !IsConnectedToServer ) return;//TODO no bool but get from tree

		PlayerPositionModel d = new PlayerPositionModel(){
			P = p.GlobalTransform,
			T = ( DateTime.Now.Ticks / 10_000 ) % 1_000_000_000// ~11.5 day
			//TODO find a better way Godot.Collections.Dictionary doesn't take long ? 
		};
		RpcId(1,"PlayerData",d.GetGodotData());
	}
}




































