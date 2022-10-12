using Microsoft.Extensions.DependencyInjection;
using Godot;
using System;

public class Bullet : KinematicBody
{

    private const int SPEED = 20;
    private Transform Origin = new Transform();
    private Vector3 Direction = Vector3.Zero;

    public Bullet SetOrigin(Transform origin){
        Origin = origin;
        return this;
    }

    //TODO static methode to get scene with like true contructor, 
    // => maybe funcion d'extensio sur node

    public override void _Ready()
    {
        //TODO use nameof
        GetNode<Timer>("Timer").Connect("timeout",this,nameof(TimeOut));
        GlobalTransform = Origin;
        Direction = - GlobalTransform.basis.z * SPEED;
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(Direction,Vector3.Up);
    }

    private void TimeOut(){
        this.QueueFree();
    }

}
