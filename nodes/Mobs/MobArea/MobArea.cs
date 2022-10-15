using System;
using Godot;

public class MobArea : Area
{
    
    //TODO : find a way to add tooltip and warning
    [Export(PropertyHint.File, "*.tscn,")]
    private string MobScencePath;    

    [Export(PropertyHint.Range, "0,20")]
    private int Number = 1;

    private const int TIME_TO_POP_MIN = 3;
    private const int TIME_TO_POP_MAX = 10;


    private PackedScene MobScene;
    private Node MobsNode;
    private bool CanPopMob = true;
    private readonly Random random = new Random();

    public override void _Ready()
    {
        MobsNode = GetNode("Mobs");
        GD.Print(MobsNode.GetClass());

        MobScene = GD.Load<PackedScene>("res://nodes/Mobs/Mob.tscn");
        for(int i = 0;i<Number;i++){
            addMob();
        }
        //TODO do  methode extension =>  addTimer(this Timer, Node, Action<>() )
        Timer t = new Timer();
        t.WaitTime = 1;
        t.Autostart = true;
        t.Connect("timeout",this,nameof(checkNumOfMob));
        AddChild(t);
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
        Node m = MobScene.Instance();
        MobsNode.AddChild(m);
    }


}
