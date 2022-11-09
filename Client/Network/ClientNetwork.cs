using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using ConnectionStatus = Godot.NetworkedMultiplayerPeer.ConnectionStatus;
using System.Threading;
using System.Threading.Tasks;
using System;
using Godot;

public partial class Network : IClientNetwork{
	private const string SERVER_IP = "127.0.0.1";
	private Boolean IsConnectedToServer = false;
	private WorldManager _WorldManager;

	private int index = 0;
	private Dictionary<int,Wrapper> Wrappers = new Dictionary<int, Wrapper>();

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
		IsConnectedToServer = true;//TODO bool useless
		SyncClock.GetInstance().InitSync();
	}

	private void ConnectionFaild(){
		GD.Print("connected faild");
		IsConnectedToServer = false;
	}
	
	public void SendPlayerData(Player p){

		if( GetTree().NetworkPeer.GetConnectionStatus() != ConnectionStatus.Connected )
			return;

		PlayerModel d = new PlayerModel(){
			Time = Time.GetTicksMsec(),
			Tr = p.GlobalTransform
		};
		RpcUnreliableId(1,nameof(PlayerData),d.GetGodotData());
	}

	[Remote]
	private void ReceiveWorlState(params object[] data){
		_WorldManager.AddWorldState(data.GetModelData<WorldStateModel>());	
	}

	public async Task<T> Request<T>(int cmd,ConvertGodoData data)where T : ConvertGodoData,new(){
		Wrapper<T> w = new Wrapper<T>();
		Wrappers.Add(index,w);
		RpcId(1,nameof(RequestServer),index,cmd,data.GetGodotData());
		index++;
		if(index > 1_000_000)	index = 0;
		await w;
		return w.Item;
	}

	/*public async Task<T> Request<T>()where T : ConvertGodoData,new(){//TODO update maybe Later
		Wrapper<T> w = new Wrapper<T>();
		Wrappers.Add(index,w);
		RpcId(1,nameof(RequestServer),0,index);
		index++;
		if(index > 1_000_000)	index = 0;//TODO useless ? 
		await w;
		return w.Item;
	}*/

	[Remote]
	public void AnswerRequest(int i,params object[] data){
		Wrappers[i].Convert(data);
		Wrappers[i].Start();
		Wrappers.Remove(i);
	}


	abstract class Wrapper :Task{
		public Wrapper():base( ()=>{} ){}
		public abstract void Convert(object[] data);
	}

	class Wrapper<T> : Wrapper where T : ConvertGodoData,new(){
		public T Item {get;private set;}
		public override void Convert(object[] data){
			GD.Print("get answer convert");
			GD.Print(data);
			GD.Print(data.Length);
			GD.Print(data[0]);
			Item = data.GetModelData<T>();
			GD.Print("converted");
		}
	}
}




































