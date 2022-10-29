using System;
using Godot;

public class MobAttckMove : IMobAttack{

    
    public int DamageMax {get;} = 0;
    public int Speed {get;set;} = 1; 
    public int Distance {get;set;} = 1; 
    public float DistanceMax{get;set;} = 0;
    public IMobGoBackIdle GoBckIdel {get;set;}


    private  Action Finish;
    private MobBase Parent;
    private Player Target;
    private Vector3 Velo = Vector3.Zero;
    private float DistanceToDo;

    public void AttackAction(float delta){

        Vector3 vl = Velo * Vector3.Up;
        vl.y -= 9.8f;
        Vector2 diff = new Vector2(Parent.GlobalTranslation.x - Target.GlobalTranslation.x ,  Parent.GlobalTranslation.z - Target.GlobalTranslation.z   );
        float a = Mathf.Atan2(diff.x,diff.y); 
        Parent.RotateY(  a - Parent.Rotation.y );
        vl += Vector3.Forward.Rotated(Vector3.Up,Parent.Rotation.y)* Speed;
        DistanceToDo -= (delta * Speed);
        Velo = Parent.MoveAndSlide(vl,Vector3.Up);
        if( DistanceToDo < 0 )
            Finish?.Invoke();
    }

    public void AddFinishAction(Action finish){
        //TODO Finish -= finish;
        Finish += finish;
    }
    public void removeFinishAction(Action finish){
        Finish -= finish;
    }
    public void Start(MobBase parent,Player target){
        Parent = parent;
        Target = target;
        DistanceToDo = Distance;
    }
    public void Reset(){

    }
    public float GetDistMinToAttackTarget(){
        return GoBckIdel?.DistanceMax ?? (float)(DistanceMax != 0 ? DistanceMax : 999); 
    }
}