using Godot;

/*
Partial Class => one part in client and another in server folder
*/

public partial class Network : Node{
	private const int PORT = 8181;

	private NetworkedMultiplayerENet NetworkPeer;
	private readonly PackedScene PlayerSkinScene = GD.Load<PackedScene>("res://nodes/Player/PlayerSkin/PlayerSkin.tscn");

	public override void _Ready()
	{
		GD.Print("init Network");
		NetworkPeer = new NetworkedMultiplayerENet();
	}

}




































