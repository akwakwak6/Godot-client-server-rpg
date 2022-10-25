using Godot;
using System;
public class MobAttckAnime: Godot.Object, IMobAttack{
    
    public string Name{get;set;}
    public int DamageMax{get;set;} = 50;
    public int Time{get;set;} = 5000;
    public float DistMinToAttackTarget{get;set;} = 1;
    protected Action Finish;
    protected readonly Delay Delay = new Delay();
    private readonly Area HitBoxArea;

    public MobAttckAnime(Area area){
        HitBoxArea = area;
    }
    public void AddFinishAction(Action finish){
        //TODO Finish -= finish;
        Finish += finish;
    }
    public void removeFinishAction(Action finish){
        Finish -= finish;
    }

    public async void Start(MobBase parent,Player target){
        HitBoxArea.Connect("body_entered",this,nameof(BodyEntered));
        parent.PlayAnimation(Name);
        await Delay.wait(Time);
        HitBoxArea.Disconnect("body_entered",this,nameof(BodyEntered));
        Finish?.Invoke();
    }

    private void BodyEntered(Node body){
        if(body is Player p){
            p.OnHit(DamageMax);
        }
    }

    public float GetDistMinToAttackTarget(){
        return DistMinToAttackTarget;
    }

    public void Reset(){
        Delay.Cancel();
    }

}