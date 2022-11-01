using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Network : IServerNetwork{

	
	private const int NB_MAX_CLIENT = 20;
	private readonly Dictionary<int,PlayerModel> _PlayersPosition = new Dictionary<int,PlayerModel>();
	private readonly Delay loop = new Delay();

    public void StartServer(){
		GD.Print("Start server");		
		NetworkPeer.CreateServer(PORT,NB_MAX_CLIENT);
		GetTree().NetworkPeer = NetworkPeer;
		GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
		GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnect));
		loop.boucle(2000,SendPlayersPosition);
	}

	private void PlayerConnected(int playerID){
		GD.Print($"Server player {playerID} connected");
		KinematicBody ps = PlayerSkinScene.Instance<KinematicBody>();
		ps.Name = playerID.ToString();
		ps.SetNetworkMaster(playerID);
		AddChild(ps,true);
	}

	private void PlayerDisconnect(int playerID){
		GD.Print($"Server player {playerID} disconnect");
		_PlayersPosition.Remove(playerID);
	}

	private void SendPlayersPosition(){
		if( _PlayersPosition.Count == 0 )	return;
		/*Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
		Godot.Collections.Dictionary players = new Godot.Collections.Dictionary();
		foreach( KeyValuePair<int,PlayerModels> pm in _PlayersPosition ){
			players.Add(pm.Key,pm.Value.P);
		}
		data.Add("Players",players);
		data.Add("T", (int) OS.GetTicksMsec());//TODO after fix issue long in godot object, use long.*/
		
		WorldStateModel m = new WorldStateModel(){
			T = (int) OS.GetTicksMsec(),
			PS = _PlayersPosition.Select( p => new  PlayerModels(){ Id = p.Key , TR = p.Value.Tr } ).ToList<PlayerModels>()
		};
		RpcUnreliableId(0,nameof(ReceiveWorlState),m.GetGodotData());
	}


	[Remote]
	private void PlayerData(params object[] d){

		PlayerModel pm = d.GetModelData<PlayerModel>();

		GD.Print(pm.Time);
		GD.Print(pm.Tr);
		GD.Print(pm.pm);
		foreach(PlayerModels f in pm.pm){
			GD.Print(f.Id);
			GD.Print(f.TR);
		}
		//GD.Print(pm.pm.Id);

		/*int pId = GetTree().GetRpcSenderId();
		KinematicBody kine = GetNode<KinematicBody>(pId.ToString());
		PlayerModel pm = data.GetModelData<PlayerModel>();
		kine.GlobalTransform = pm.Tr;
		if(_PlayersPosition.ContainsKey(pId)){
			if(_PlayersPosition[pId].Time < pm.Time){
				_PlayersPosition[pId] = pm;
			}
		}else{
			_PlayersPosition.Add(pId,pm);
		}*/

	}

}

