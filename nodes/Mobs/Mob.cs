using Godot;
using System;

public class Mob : KinematicBody
{
    private AnimationNodeStateMachinePlayback playBack;

    private int v = 100;

    public override void _Ready(){
        AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
        playBack = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        playBack.Start("idle");

        GetNode<Timer>("Timer").Connect("timeout",this,"tictoc");
        //HealtBar

    }

    private void tictoc(){
        GetNode<HealtBar>("HealtBar").Value = v;
        v-=10;
    }

}
