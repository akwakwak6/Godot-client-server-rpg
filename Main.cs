using Microsoft.Extensions.DependencyInjection;
using Godot;
using System;

/*
	cmds

=> export 
C:\Users\misso\OneDrive\Documents\Godot\Godot_v3.5.1-stable_mono_win64\Godot_v3.5.1-stable_mono_win64/Godot_v3.5.1-stable_mono_win64.exe --export-debug windows ./out/server.exe --no-window

=> execute
./out/server.exe server

=> build
C:\Users\misso\OneDrive\Documents\Godot\Godot_v3.5.1-stable_mono_win64\Godot_v3.5.1-stable_mono_win64/Godot_v3.5.1-stable_mono_win64.exe --build-solutions --no-window -q

=> start
C:\Users\misso\OneDrive\Documents\Godot\Godot_v3.5.1-stable_mono_win64\Godot_v3.5.1-stable_mono_win64/Godot_v3.5.1-stable_mono_win64.exe --resolution 200x200  server

*/

public class Main : Node
{

	Camera c;

	public override void _Ready()
	{
		IServerNetwork serverNetwork = this.GetServiceFromIOC<IServerNetwork>();
		IClientNetwork clientNetwork = this.GetServiceFromIOC<IClientNetwork>();

		if ( Array.Exists(Godot.OS.GetCmdlineArgs(), element => element == "server"))
		{
			serverNetwork.StartServer();
			c = new Camera();
			//connet enter in tree
			c.Connect("tree_entered",this, "addVector" );
			AddChild(c);
		}else
		{
			clientNetwork.StartClient();
			Player p = GD.Load<PackedScene>("res://nodes/Player/Player.tscn").Instance<Player>();
			AddChild(p);
		}
		
	}
	private void addVector(){
		c.GlobalTranslation = new Vector3(0,2.2f,6.6f);
	}

	public void test([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = ""){
		GD.Print("=> ");
		GD.Print(sourceFilePath);
	}
}
