using System.Collections.Generic;
using Godot;

public interface IMobCompAggro{

    public IMobSelectAttack SelectAttack{get;set;}
    public IMobSelectTarget SelectTarget{get;set;}
    public bool AggroAction(MobTargets targets,float delta);
    public void OnOutOfAgrroZone(Spatial zone);
    public void Reset();
}



