using Godot;
using System;

public class Bullet : KinematicBody
{

    private const int SPEED = 5;
    private Transform Origin = new Transform();
    private Vector3 Direction = Vector3.Zero;

    public Bullet SetOrigin(Transform origin){
        Origin = origin;
        return this;
    }

    public override void _EnterTree()
    {
        GlobalTransform = Origin;
        Direction = - GlobalTransform.basis.z * SPEED;
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(Direction,Vector3.Up);
    }

    public override void _Ready()
    {
        GetNode<Timer>("Timer").Connect("timeout",this,"TimeOut");
    }

    private void TimeOut(){
        this.QueueFree();
    }

}