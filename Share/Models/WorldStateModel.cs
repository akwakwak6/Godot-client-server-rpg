using System.Collections.Generic;
using Godot;

public class WorldStateModel : ConvertGodoData{

    public List<PlayerServer> PS {get;set;}
    public ulong T {get;set;}

}

public class PlayerServer : ConvertGodoData{

    public Transform TR {get;set;}
    public int Id {get;set;}

}