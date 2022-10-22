using System;
using Godot;

public class MobArea : Area
{
    
    //TODO : find a way to add tooltip and warning
    [Export(PropertyHint.File, "*.tscn,")]
    private string MobScencePath;    

    [Export(PropertyHint.Range, "0,20")]
    private int Number = 1;

    private const int TIME_TO_POP_MIN = 1;
    private const int TIME_TO_POP_MAX = 2;


    private PackedScene MobScene;
    private Node MobsNode;
    private bool CanPopMob = true;
    private readonly Random random = new Random();

    public override void _Ready()
    {
        MobsNode = GetNode("Mobs");

        MobScene = GD.Load<PackedScene>(MobScencePath);

        Connect("body_exited",this,nameof(OnBodyExited));
        Connect("body_entered",this,nameof(OnBodyEntered));
        
        Timer t = new Timer();
        t.WaitTime = 1;
        t.Autostart = true;
        AddChild(t);
        //TODO use C# task
        t.Connect("timeout",this,nameof(checkNumOfMob));
    }

    private void OnBodyExited(Node b){
        if(b is IMob mob)
            mob.OnOutOfIdleZone(this);
    }
    private void OnBodyEntered(Node b){
        if(b is IMob mob)
            mob.OnEnterIdleZone(this);
    }

    private void checkNumOfMob(){

        if( !CanPopMob ) return;

        if( MobsNode.GetChildCount() < Number ){
            CanPopMob = false;
            addMob(true);
        }

    }

    private async void addMob(bool timer = false){
        if(timer)
            await ToSignal(GetTree().CreateTimer(random.Next(TIME_TO_POP_MIN,TIME_TO_POP_MAX)), "timeout");
        CanPopMob = true;
        MobsNode.AddChild(MobScene.Instance());
        //TODO random or fix place
    }


}
