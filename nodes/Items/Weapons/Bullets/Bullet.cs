using Godot;

public class Bullet : KinematicBody
{

    private const int SPEED = 20;
    private Transform Origin = new Transform();
    private Vector3 Direction = Vector3.Zero;
    private CollisionShape _CollisionShape;

    public Bullet SetOrigin(Transform origin){
        Origin = origin;
        return this;
    }

    public override void _Ready()
    {
        GetNode<Timer>("Timer").Connect("timeout",this,nameof(TimeOut));
        GetNode<Area>("Detector").Connect("body_entered",this,nameof(HitBody));
        GlobalTransform = Origin;
        Direction = - GlobalTransform.basis.z * SPEED;
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(Direction);
    }

    private void HitBody(Node body){
        if( body is Mob mob){
            mob.OnHit(20);
        }else{
            GD.Print("not a mob bye");
        }
        this.QueueFree();
    }

    private void TimeOut(){
        this.QueueFree();
    }

}
