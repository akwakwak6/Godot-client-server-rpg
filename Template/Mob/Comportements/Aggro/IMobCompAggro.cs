using System.Collections.Generic;
using Godot;

public interface IMobCompAggro{
    public bool AggroAction(Dictionary<Player,int> targets,float delta);
    public void OnOutOfAgrroZone(Spatial zone);
    public void Reset();
}



