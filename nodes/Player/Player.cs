using Godot;

public class Player : KinematicBody
{
    private IClientNetwork ClientNetwork;
    private const int SPEED = 10;
    private const int TURN_SPEED = 3;
    private const int JUMP_VELO = 20;
    private const int GRAVITY = 1;
    private const int Y_MIN_BEFORE_TP_TO_ORIGIN = -10;
    private Camera _Camera;
    private int HP = 100;
    private readonly PackedScene BulletScene = GD.Load<PackedScene>("res://nodes/Items/Weapons/Bullets/Bullet.tscn");
    private readonly  Environment DeadSky = GD.Load<Environment>("res://nodes/Player/DeadSky.tres");
    private Vector3 Velo = Vector3.Zero;

    public override void _Ready()
    {
        GetNode<Timer>("SendData").Connect("timeout",this,"sendData");
        ClientNetwork = this.GetServiceFromIOC<IClientNetwork>();
        PlayerData pd = this.GetServiceFromIOC<PlayerData>();
        pd.SetPlayer(GetNode<Camera>("Camera"),this);
        _Camera = GetNode<Camera>("Camera");
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionPressed("ui_accept")){
            Bullet b =  BulletScene.Instance<Bullet>();

            Transform globalPosition = GlobalTransform;
            //vecter to move up and forward ( 0.5m )
            Vector3 trans = new Vector3(0,0.5f,-0.3f);
            // turn vector fct this node position 
            // add this vector to origine
            globalPosition.origin += trans.Rotated(Vector3.Up,Rotation.y);
            b.SetOrigin(globalPosition);
            b.SetSender(this);
            GetParent().AddChild(b);
        }
    }

    private void sendData(){
        PlayerModel pm = new PlayerModel(){
            Time = (int)OS.GetTicksMsec(),//TODO actually can not use long in godot object, find a way.
            Tr = GlobalTransform
        };
        ClientNetwork.SendPlayerData(this);
    }

    public void AggroMob(IMob mob){
        //mob.ConnectIdie(OnKilledMob);
    }

    private void OnKilledMob(IMob mob){
        GD.Print("I Kill the mob ");
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
            RotateY(delta * rotation/2 );

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

    public async void OnHit(int damage){
        GD.Print("player took "+damage);
        HP -= damage;
        if (HP <= 0){
            GD.Print("Player dead");
            _Camera.Environment = DeadSky;
            await ToSignal(GetTree().CreateTimer(5), "timeout");
            _Camera.Environment = null;
            HP = 100;
        }
    }

}
