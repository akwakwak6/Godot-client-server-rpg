using Godot;
using System;

public class MobAttackZone{

    public Action Finish {get;set;}
    public int DamageMax {get;set;} = 2;
    public float Radius {get;set;} = 2;
    public float Height {get;set;} = 2;

    private readonly PackedScene MobAttackSceneZone = GD.Load<PackedScene>("res://Template/Mob/Comportements/Attack/Attacks/MobAttackSceneZone.tscn");
    private readonly Delay Delay = new Delay();

    public async void Start(Node p){
        
        GD.Print("start attack");
        MobAttackSceneZone b =  MobAttackSceneZone.Instance<MobAttackSceneZone>();
        b.DamageMax = DamageMax;
        b.Radius = Radius;
        b.Height = Height;
        p.AddChild(b);
        await Delay.wait(3000);
        b.Hit();
        b.QueueFree();
        await Delay.wait(3000);
        Finish?.Invoke();
    }

    public void Reset(){
        Delay.Cancel();
    }
    
}