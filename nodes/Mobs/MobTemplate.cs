using System.Data;
using Godot;

public class MobTemplate : KinematicBody,IMob {

    private enum States {Idle,Aggro,Dead};
    private States state = States.Idle;

    public virtual void OnOutOfIdleZone(Spatial zone){

    }
    public virtual void OnOutOfAgrroZone(Spatial zone){

    }
    public virtual void OnHit(int damage){
        state = States.Aggro;
    }
    public virtual bool IsAggro(){
        return state == States.Aggro;
    }

}