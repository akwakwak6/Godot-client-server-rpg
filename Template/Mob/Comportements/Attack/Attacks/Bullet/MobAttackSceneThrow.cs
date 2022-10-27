using Godot;
using System;

public class MobAttackSceneThrowPara : MobAttackSceneParaBase{
    
    public float Speed {get;set;} = 2;
    public Transform Origin {get;set;}
    public int Distance {get;set;} = 10;
    public override int DamageMax {get;set;} = 23;

}

public class MobAttackSceneThrow : KinematicBody,IMobAttackScene
{
    private MobAttackSceneThrowPara p;
    private Vector3 Direction = Vector3.Zero;

    private float Distance;
    private bool started = false;

    public void setPara(MobAttackSceneParaBase paras){
        p = (MobAttackSceneThrowPara) paras;
    }
    public override void _Ready(){
        SetPhysicsProcess(false);
        Distance = p.Distance;
        GetNode("Area").Connect( "body_entered",this, nameof(BulletHit));
    }

    private void BulletHit(Node body){
        if(!started) return;
        if(body is Player player){
            player.OnHit(p.DamageMax);
        }else if(! (body is IMob) ){
            QueueFree();
        }

    }

    public override void _PhysicsProcess(float delta){
        MoveAndSlide(Direction);
        Distance -= (delta * p.Speed);
        if(Distance < 0)  QueueFree();
    }

    public void Hit(){
        started = true;
        Direction = - GlobalTransform.basis.z * p.Speed;
        SetPhysicsProcess(true);
    }

}
