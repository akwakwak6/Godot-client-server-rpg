using Godot;
using System;

public abstract class AbstMobAttack{

    public Action Finish {get;set;}
    public int DamageMax {get;set;} = 2;
    public int TimeToHit {get;set;} = 2000;
    public int TimeAfterHit {get;set;} = 2000;
    protected readonly PackedScene SceneResource;
    protected readonly Delay Delay = new Delay();
    
    public AbstMobAttack(string path){
        SceneResource = GD.Load<PackedScene>(path);
    }

    public async virtual void Start(MobBase parent){
        
        Node n =  SceneResource.Instance();
        IMobAttckScene s = (IMobAttckScene) n;
        s.DamageMax = DamageMax;
        initScene(s);
        //s.init();
        parent.AddChild( n );
        await Delay.wait(TimeToHit);
        s.Hit();
        n.QueueFree();
        await Delay.wait(TimeAfterHit);
        Finish?.Invoke();
    }

    protected abstract void initScene(IMobAttckScene s);

    public virtual void Reset(){
        Delay.Cancel();
    }


}