using System.Collections.Generic;
using System;
using Godot;
public class MobAttackInitScene<S> : IMobAttack where S : Node,IMobAttackScene{

    //TODO use C# 8 and use static in IMobAttackScene interface => S.Path
    protected static Dictionary<Type,string> Paths = new Dictionary<Type, string>(){
        {typeof(MobAttackSceneZone),"res://Template/Mob/Comportements/Attack/Attacks/Zone/MobAttackSceneZone.tscn"},
        {typeof(MobAttackSceneThrow),"res://Template/Mob/Comportements/Attack/Attacks/Bullet/MobAttackSceneThrow.tscn"}
    };

    protected Action Finish;
    protected MobAttackSceneParaBase Paras;

    protected readonly PackedScene SceneResource;
    protected readonly IMobAttackScene Scene;
    protected readonly Delay Delay = new Delay();

    public MobAttackInitScene(MobAttackSceneParaBase paras){
        Paras = paras;
        SceneResource = GD.Load<PackedScene>(Paths[typeof(S)]);
    }

    public void AddFinishAction(Action finish){
        //TODO Finish -= finish;
        Finish += finish;
    }
    public void removeFinishAction(Action finish){
        Finish -= finish;
    }
    public virtual async void Start(MobBase parent,Player target){
        S n =  SceneResource.Instance<S>();
        n.setPara(Paras);
        parent.AddChild(n);
        parent.PlayAnimation(Paras.Name);
        await Delay.wait(Paras.TimeToHit);
        n.Hit();
        n.QueueFree();
        await Delay.wait(Paras.Time - Paras.TimeToHit);
        Finish?.Invoke();
    }

    public void Reset(){
        Delay.Cancel();
    }

    public float GetDistMinToAttackTarget(){
        return Paras.DistMinToAttackTarget;
    }

}








