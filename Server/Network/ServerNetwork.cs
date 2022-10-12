using Godot;

public partial class Network : IServerNetwork{

	private const int NB_MAX_CLIENT = 20;

    public void StartServer(){
		GD.Print("Start server");
		NetworkPeer.CreateServer(PORT,NB_MAX_CLIENT);
		GetTree().NetworkPeer = NetworkPeer;
		GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
		GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnect));
	}

	private void PlayerConnected(int playerID){
		GD.Print($"Server player {playerID} connected");
		KinematicBody ps = PlayerSkinScene.Instance<KinematicBody>();
		ps.Name = playerID.ToString();
		ps.SetNetworkMaster(playerID);
		AddChild(ps);
	}

	private void PlayerDisconnect(int playerID){
		GD.Print($"Server player {playerID} disconnect");
	}


	[Remote]
	public void PlayerData(Transform d){
		int pId = GetTree().GetRpcSenderId();
		GetNode<KinematicBody>(pId.ToString()).GlobalTransform = d;
	}

}

