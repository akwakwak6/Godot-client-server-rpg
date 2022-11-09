using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Network : IServerNetwork{

	private const int NB_MAX_CLIENT = 20;
	private readonly Dictionary<int,PlayerModel> _PlayersPosition = new Dictionary<int,PlayerModel>();
	private readonly Delay loop = new Delay();

	private IrpcServer rpcServer;

    public void StartServer(){
		GD.Print("Start server");		
		NetworkPeer.CreateServer(PORT,NB_MAX_CLIENT);
		GetTree().NetworkPeer = NetworkPeer;
		GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
		GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnect));
		loop.boucle(35,SendPlayersPosition);
		rpcServer = this.GetServiceFromIOC<IrpcServer>();
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
		if(HasNode(playerID.ToString())){
			GetNode(playerID.ToString()).QueueFree();
		}
	}

	private void SendPlayersPosition(){
		if( _PlayersPosition.Count == 0 )	return;
		WorldStateModel m = new WorldStateModel(){
			T = OS.GetTicksMsec(),
			PS = _PlayersPosition.Select( p => new PlayerServer(){ Id = p.Key , TR = p.Value.Tr } ).ToList<PlayerServer>()
		};

		foreach(var ps in  _PlayersPosition ){
			if(HasNode(ps.Key.ToString())){
				GetNode<KinematicBody>(ps.Key.ToString()).GlobalTransform = ps.Value.Tr;
			}
		}
		RpcUnreliableId(0,nameof(ReceiveWorlState),m.GetGodotData());
	}

	[Remote]
	private void PlayerData(params object[] d){

		int id = GetTree().GetRpcSenderId();
		PlayerModel pm = d.GetModelData<PlayerModel>();
		if(_PlayersPosition.ContainsKey(id)){
			if( _PlayersPosition[id].Time < pm.Time ){
				_PlayersPosition[id] = pm;
			}
		}else{
			_PlayersPosition.Add(id,pm);
		}
		
	}

	[Remote]
	private void RequestServer(int index,int cmd,params object[] data){
		int id = GetTree().GetRpcSenderId();
		//data[0] += " checked ";
		GD.Print("before "+cmd);
		ConvertGodoData ret =   rpcServer.Call(cmd,data)  ;
		//rpcServer.Call(cmd,data);
		GD.Print("after");
		GD.Print(ret);
		RpcId(id, nameof(AnswerRequest),index, ret.GetGodotData() );
	}

	/*[Remote]
	private void RequestServer(int index,int cmd){
		int id = GetTree().GetRpcSenderId();
		Tp tp = new Tp(){para = "OKi good enought"};
		RpcId(id, nameof(AnswerRequest),index , tp.GetGodotData() );
	}*/

}

