using System.Collections.Generic;
using States = IMob.States;
using System;
using Godot;

[Tool]
public class MobBase : KinematicBody,IMob {


    //TODO methode protected StatIdle,Stat
    private Action<IMob> SignalDead;

    [Export]
    private readonly NodePath healtBarPath = "HealtBar";
    [Export]
    private readonly int MaxHP = 10;


    public States State{get;protected set;}


    private int HP;
    private Vector3 Velocity = Vector3.Zero;
    private HealtBar healtBar;

    //TODO maybe use set get to have list ?
    private Dictionary<Player,int> Targets = new Dictionary<Player,int>();

    protected IMobCompIdle CompIdle;
    protected IMobCompAggro compAggro;

    public override void _Ready(){
        healtBar = GetNode<HealtBar>(healtBarPath);
        HP = MaxHP;
    }

    public override void _PhysicsProcess(float delta)
    {
        if( GlobalTranslation.y < -10 ) {
            QueueFree();
            return;
        }

        if( State== States.Idle )
            CompIdle?.IdleAction(delta);
        else if(State== States.Aggro){
            if(compAggro?.AggroAction(Targets,delta)??false){
                SwitchToIdle();
            }
        }
    }

    public virtual void OnOutOfIdleZone(Spatial zone){
        CompIdle?.OnOutOfIdleZone(zone);
    }
    public virtual void OnEnterIdleZone(Spatial zone){
        CompIdle?.OnEnterInIdleZone(zone);
    }
    public virtual void OnOutOfAgrroZone(Spatial zone){
        if(State== States.Dead)    return;
        GD.Print("in OnOutOfAgrroZone");
        CompIdle?.Reset();
        State= States.Idle; 
    }

    public virtual void OnHit(int damage,Player p){

        if(State== States.Dead)    return;
        
        HP -= damage;
        SwitchToAggro();

        if(HP > 0){
            healtBar.Value = (float)HP/MaxHP;
            if(Targets.ContainsKey(p))
                Targets[p]+= damage;
            else
                Targets.Add(p,damage);
            return;
        }
        die();

    }

    //TODO definir compt when switch stat in set state
    protected void SwitchToAggro(){
        if(States.Dead == State)    return;
        if(States.Aggro == State)   return;
        compAggro?.Reset();
        State= States.Aggro;
        healtBar.Visible = true;
    }

    protected void SwitchToIdle(){
        if(States.Dead == State)    return;
        if(States.Idle == State)   return;
        CompIdle?.Reset();
        State= States.Idle;
        healtBar.Visible = false;
        HP = MaxHP;
    }

    private void die(){
        healtBar.QueueFree();
        State= States.Dead;
        SignalDead?.Invoke(this);
        //await ToSignal(GetTree().CreateTimer(3),"timeout");
        QueueFree();
    }

    public Dictionary<Player,int> getTargets(){
        return Targets;
    }

}