using System.Collections.Generic;
using Godot;

public class CompAggroGoTarget : IMobCompAggro{

    public int Speed {get;set;} = 1;

    //TODO put in interface Aggro IsFinish
    public int DistToSwitchIdle {get;set;} = 4;
    public float KeepTargetDistance {get;set;} = 1;

    public IMobSelectAttack SelectAttack{get;set;} = new MobSelectRandomAttack();
    public IMobSelectTarget SelectTarget{get;set;} = new MobSelectFirstTarget();
    public IMobGoBackIdle GoBackIdle{get;set;} = new MobGoBackIdle();

    protected readonly KinematicBody parent;
    protected Vector3 Velo = Vector3.Zero;


    public CompAggroGoTarget(KinematicBody _parent){
        parent = _parent;
    }
    
    public virtual bool AggroAction(MobTargets targets,float delta){

        //TODO resetTarget
        //TODO check GoBackIdle ?

        Vector3 vl = Velo * Vector3.Up;
        vl.y -= 9.8f;

        Player p = targets.GetList()[0];//TODO use select Target
        float distanceTarget = p.GlobalTranslation.DistanceTo(parent.GlobalTranslation);
        //TODO check distance  parent(x,z) - target(x,y)
        //TODO stop try to forward if obstacle ( raycast ? )
        if(DistToSwitchIdle < distanceTarget)   return true;
        
        

        Vector2 diff = new Vector2(parent.GlobalTranslation.x - p.GlobalTranslation.x ,  parent.GlobalTranslation.z - p.GlobalTranslation.z   );
        float a = Mathf.Atan2(diff.x,diff.y); 
        parent.RotateY(  a - parent.Rotation.y );

        if(KeepTargetDistance < distanceTarget){
            vl += Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y)* Speed;
        } 

        Velo = parent.MoveAndSlide(vl,Vector3.Up);
        
        return false;
    }

    public void OnOutOfAgrroZone(Spatial zone){

    }

    public virtual void Reset(){
        Velo = Vector3.Zero;
    }
}