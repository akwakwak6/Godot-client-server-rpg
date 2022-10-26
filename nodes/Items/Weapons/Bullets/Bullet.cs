using Godot;

public class Bullet : KinematicBody
{

    private const int SPEED = 20;
    private Transform Origin = new Transform();
    private Vector3 Direction = Vector3.Zero;
    private CollisionShape _CollisionShape;

    private Player player;

//TODO net set bur property
    public Bullet SetOrigin(Transform origin){
        Origin = origin;
        return this;
    }
    public Bullet SetSender(Player p){
        player = p;
        return this;
    }

    public override void _Ready()
    {
        //TODO user C# task
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
        if( body is IMob mob){
            player.AggroMob(mob);
            mob.OnHit(20,player);
        }else
            GD.Print("not a mob");
        this.QueueFree();
    }

    private void TimeOut(){
        this.QueueFree();
    }

}
