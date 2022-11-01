using Godot;
using System;
using System.Collections.Generic;

public partial class Network : IClientNetwork{
	private const string SERVER_IP = "127.0.0.1";
	private Boolean IsConnectedToServer = false;
	private WorldManager _WorldManager;

	public void StartClient(){
		GD.Print("Start client");
		NetworkPeer = new NetworkedMultiplayerENet();
		NetworkPeer.CreateClient(SERVER_IP,PORT);
		this.GetTree().NetworkPeer = NetworkPeer;
		NetworkPeer.Connect("connection_succeeded", this,nameof(ConnectionSucceeded));
		NetworkPeer.Connect("connection_failed", this, nameof(ConnectionFaild));
		_WorldManager = this.GetServiceFromIOC<WorldManager>();
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

		List<PlayerModels> l = new List<PlayerModels>();
		l.Add(new PlayerModels(){
				Id = 67,
				TR = p.GlobalTransform
			});
		l.Add(new PlayerModels(){
				Id = 89,
				//TR = p.GlobalTransform
			});
		
		PlayerModel d = new PlayerModel(){
			Time = (int) Time.GetTicksMsec(),
			Tr = p.GlobalTransform,
			//pm = l
		};
		//d.GetGodotData();
		RpcUnreliableId(1,nameof(PlayerData),d.GetGodotData());
	}

	[Remote]
	private void ReceiveWorlState(Godot.Collections.Dictionary data){
		GD.Print("get data");
		//_WorldManager.AddWorldState(data.GetModelData<WorldStateModel>());		
	}
}




































