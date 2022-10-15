using Godot;
using System;

public class AggroArea : Area
{

    private  Action<Player> DetectPlayer;

    public override void _Ready()
    {   
        this.Connect("body_entered",this,nameof(checkBody));   
    }

    public void ConnectToAggroPkayer(Action<Player> p){
        DetectPlayer += p;
    }

    private void checkBody(Node body){
        if( body is Player p ){
            DetectPlayer?.Invoke(p);
        }
    }

}
