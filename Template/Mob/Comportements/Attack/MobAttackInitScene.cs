using System.Collections.Generic;
using System;
using Godot;
public class MobAttackInitScene<S> : IMobAttack where S : Node,IMobAttackScene{

    //TODO use C# 8 and use static in IMobAttackScene interface => S.Path
    private static Dictionary<Type,string> Paths = new Dictionary<Type, string>(){
        {typeof(MobAttackSceneZone),"res://Template/Mob/Comportements/Attack/Attacks/Zone/MobAttackSceneZone.tscn"}
    };

    protected Action Finish;
    private AbstMobAttackScenePara Paras;

    protected readonly PackedScene SceneResource;
    protected readonly IMobAttackScene Scene;
    protected readonly Delay Delay = new Delay();

    public MobAttackInitScene(AbstMobAttackScenePara paras){
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
    public virtual async void Start(MobBase parent){
        S n =  SceneResource.Instance<S>();
        n.setPara(Paras);
        parent.AddChild(n);
        await Delay.wait(Paras.TimeToHit);
        n.Hit();
        n.QueueFree();
        await Delay.wait(Paras.Time - Paras.TimeToHit);
        Finish?.Invoke();
    }

    public void Reset(){
        Delay.Cancel();
    }

}








