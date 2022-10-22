using System.Collections.Generic;
using Godot;

public class CompAggroGoTarget : IMobCompAggro{

    public int Speed {get;set;} = 1;
    public int DistToSwitchIdle {get;set;} = 4;
    public float TargetDistance {get;set;} = 1;

    private readonly KinematicBody parent;
    private Vector3 Velo = Vector3.Zero;


    public CompAggroGoTarget(KinematicBody _parent){

        parent = _parent;

    }
    
    public bool AggroAction(Dictionary<Player,int> targets,float delta){
        
        //TODO maybe put back this instruction if error 
        //if(targets.Count == 0)  return false;

        Vector3 vl = Velo * Vector3.Up;
        vl.y -= 9.8f;

        var e = targets.GetEnumerator();
        e.MoveNext();
        float distanceTarget = e.Current.Key.GlobalTranslation.DistanceTo(parent.GlobalTranslation);
        if(DistToSwitchIdle < distanceTarget)   return true;
        
        //TODO fix issue when play jump
        parent.LookAt(e.Current.Key.GlobalTranslation,Vector3.Up);
        /*Vector3 r = parent.RotationDegrees;
        r.y = Mathf.Lerp( parent.RotationDegrees.y, Mathf.Rad2Deg(Mathf.Atan2( e.Current.Key.GlobalTranslation.x, e.Current.Key.GlobalTranslation.z )), 1 ) ;
        parent.RotationDegrees = r;*/

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