using System.Linq;
using System.Collections.Generic;
using States = IMob.States;
using System;
using Godot;


public class MobBase : KinematicBody,IMob {


    //TODO methode protected StatIdle,Stat
    private Action<IMob> SignalDead;

    [Export]
    private readonly NodePath animationPlayerPath = "AnimationPlayer";
    [Export]
    private readonly NodePath healtBarPath = "HealtBar";
    [Export]
    private readonly int MaxHP = 10;


    public States State{get;protected set;}

    private int HP;
    private Vector3 Velocity = Vector3.Zero;
    private HealtBar healtBar;
    private MobTargets Targets;
    protected IMobCompIdle CompIdle;
    protected IMobCompAggro compAggro;
    protected AnimationPlayer player;

    public override void _Ready(){
        player = GetNode<AnimationPlayer>(animationPlayerPath);
        healtBar = GetNode<HealtBar>(healtBarPath);
        Targets = new MobTargets(this);
        HP = MaxHP;

    }

    public override void _PhysicsProcess(float delta)
    {
        if( GlobalTranslation.y < -10 ) {
            die();
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
            Targets.PlayerHit(p,damage);
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
        healtBar.QueueFree(); //TODO add ? to check null
        State= States.Dead;
        SignalDead?.Invoke(this);
        CompIdle?.Reset();
        compAggro?.Reset();
        //TODO reset
        //
        //await ToSignal(GetTree().CreateTimer(3),"timeout");
        QueueFree();
    }

    public MobTargets GetTargets(){
        return Targets;
    }
    public List<Player> GetTargetList(){
        return Targets.GetList();
    }

    public void PlayAnimation(string name){
        player?.Play(name);
    }

    public virtual Spatial GetWheapon(){
        throw new Exception("GetWheapon is not implemented, override it");
    }

}