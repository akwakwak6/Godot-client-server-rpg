using Godot;

public class Player : KinematicBody
{
    private IClientNetwork ClientNetwork;
    private const int SPEED = 10;
    private const int TURN_SPEED = 5;
    private const int JUMP_VELO = 20;
    private const int GRAVITY = 1;
    private const int Y_MIN_BEFORE_TP_TO_ORIGIN = -10;

    public readonly PackedScene BulletScene = GD.Load<PackedScene>("res://nodes/Items/Weapons/Bullets/Bullet.tscn");
    private Vector3 Velo = Vector3.Zero;

    public override void _Ready()
    {
        GetNode<Timer>("SendData").Connect("timeout",this,"sendData");
        ClientNetwork = this.GetServiceFromIOC<IClientNetwork>();
        PlayerData pd = this.GetServiceFromIOC<PlayerData>();
        pd.SetPlayer(GetNode<Camera>("Camera"));
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionPressed("ui_accept")){
            Bullet b =  BulletScene.Instance<Bullet>();
            b.SetOrigin(GlobalTransform);
            GetParent().AddChild(b);
        }
    }

    private void sendData(){
        ClientNetwork.SendPlayerData(GlobalTransform);
    }


    public override void _PhysicsProcess(float delta)
    {
        if( GlobalTranslation.y < Y_MIN_BEFORE_TP_TO_ORIGIN ){
            GlobalTranslation = Vector3.Zero;
            return;
        }

        int rotation = 0;
        if (Input.IsActionPressed("ui_left"))
            rotation+=TURN_SPEED;
        if (Input.IsActionPressed("ui_right"))
            rotation-=TURN_SPEED;
        if( rotation != 0 )
            RotateY(delta * rotation);

        Velo *= Vector3.Up;
        if (Input.IsActionPressed("ui_up"))
            Velo.z = -SPEED;
        if (Input.IsActionPressed("ui_down"))
            Velo.z = SPEED;
        
        if ( IsOnFloor() ){
            if (Input.IsActionPressed("ui_select")){
                Velo.y = JUMP_VELO;  
            }
        }else{
            Velo.y -= GRAVITY;
        }
        
        if (Velo.Length() > 0){
            Velo = Velo.Rotated(Vector3.Up,Rotation.y);
            Velo = MoveAndSlide(Velo,Vector3.Up);
        }
    }

}
