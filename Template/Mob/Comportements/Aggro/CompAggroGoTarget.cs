using System.Collections.Generic;
using Godot;

public class CompAggroGoTarget : IMobCompAggro{

    public int Speed {get;set;} = 1;
    public int DistToSwitchIdle {get;set;} = 4;
    public float TargetDistance {get;set;} = 1;

    protected readonly KinematicBody parent;
    protected Vector3 Velo = Vector3.Zero;


    public CompAggroGoTarget(KinematicBody _parent){
        parent = _parent;
    }
    
    public virtual bool AggroAction(Dictionary<Player,int> targets,float delta){
        
        //TODO maybe put back this instruction if error 
        //if(targets.Count == 0)  return false;

        Vector3 vl = Velo * Vector3.Up;
        vl.y -= 9.8f;

        var e = targets.GetEnumerator();
        e.MoveNext();
        float distanceTarget = e.Current.Key.GlobalTranslation.DistanceTo(parent.GlobalTranslation);
        //TODO check distance  parent(x,z) - target(x,y)
        //TODO stop try to forward if obstacle ( raycast ? )
        if(DistToSwitchIdle < distanceTarget)   return true;
        
        

        Vector2 diff = new Vector2(parent.GlobalTranslation.x - e.Current.Key.GlobalTranslation.x ,  parent.GlobalTranslation.z - e.Current.Key.GlobalTranslation.z   );
        float a = Mathf.Atan2(diff.x,diff.y); 
        parent.RotateY(  a - parent.Rotation.y );

        if(TargetDistance < distanceTarget){
            vl += Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y)* Speed;
        } 

        Velo = parent.MoveAndSlide(vl,Vector3.Up);
        
        return false;
    }

    public void OnOutOfAgrroZone(Spatial zone){

    }

    public void Reset(){
        Velo = Vector3.Zero;
    }
}