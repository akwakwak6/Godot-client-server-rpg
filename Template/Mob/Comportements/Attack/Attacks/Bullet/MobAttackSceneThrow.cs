using Godot;
using System;

public class MobAttackSceneThrowPara : MobAttackSceneParaBase{
    
    public float Speed {get;set;} = 2;
    public Transform Origin {get;set;}

}

public class MobAttackSceneThrow : KinematicBody,IMobAttackScene
{
    private MobAttackSceneThrowPara p;
    private Vector3 Direction = Vector3.Zero;
    //private readonly Delay Delay = new Delay();

    public void setPara(MobAttackSceneParaBase paras){
        p = (MobAttackSceneThrowPara) paras;
    }
    public override void _Ready()
    {
        SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(Direction);

    }

    public void Hit(){
        Direction = - GlobalTransform.basis.z * p.Speed;
        SetPhysicsProcess(true);
    }

}
