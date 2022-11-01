using gdt = Godot.Collections;
using System.Collections.Generic;
using Godot;

public class WorldManager{

    private readonly List<gdt.Dictionary> Worlds = new List<gdt.Dictionary>();

    public void AddWorldState(WorldStateModel w){
        GD.Print("add World state");
    }

}