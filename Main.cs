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

	public override void _Ready()
	{
		Network network = GetNode<Network>("/root/Network");

		if ( Array.Exists(Godot.OS.GetCmdlineArgs(), element => element == "server"))
		{
			network.StartServer();
		}else
		{
			network.StartClient();
		}
		
	}

}
