using Godot;
using System;


public class Mob : KinematicBody
{
    private AnimationNodeStateMachinePlayback playBack;
    private HealtBar lifeDisplay;
    private readonly Random random = new Random();


    private int HP = 100;
    private int TURN_SPEED = 1;
    private int SPEED = 5;
    private const int MoveMin = 1,MoveMax = 10,TimerToMoveMin = 3,TimerToMoveMax = 7;

    //public readonly PackedScene AttackScene = GD.Load<PackedScene>("res://nodes/Mobs/Attack/Attack.tscn");


    private bool IsMovingToAttack = false;
    private bool IsDead = false;
    private bool IsAggro = false;
    private bool IsAttacking = false;
    private bool MoveRandom = true,MoveGoBack = false;
    private float RandomMoveAngle = (float)GD.RandRange( 0 , Math.PI );
    private float RandomMoveDistance = (float)GD.RandRange( MoveMin , MoveMax );
    private Timer waiterTimer;

    private float DistanceToAttack;

    private SpatialMaterial IdleColorEyes = new SpatialMaterial();
    private SpatialMaterial AggroColorEyes = new SpatialMaterial();

    private MeshInstance RightEye;
    private MeshInstance LeftEye;

    private Spatial Target;

    public override void _Ready(){
        waiterTimer = new Timer();
        waiterTimer.OneShot = true;
        AddChild(waiterTimer);
        AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
        playBack = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        playBack.Start("Idle");
        lifeDisplay = GetNode<HealtBar>("../HealtBar");
        RightEye = GetNode<MeshInstance>("Eyes/RightEye");
        LeftEye = GetNode<MeshInstance>("Eyes/LeftEye");
        IdleColorEyes.AlbedoColor = new Color(0,0,0);
        AggroColorEyes.AlbedoColor = new Color(1,0,0);

        RightEye.SetSurfaceMaterial(0,IdleColorEyes);
        LeftEye.SetSurfaceMaterial(0,IdleColorEyes);


        GetParent().GetNode<AggroArea>("AggroArea").ConnectToAggroPkayer(OnPLayerClose);
        /*Timer t = new Timer();
        t.WaitTime = 8;
        t.Autostart = true;
        t.Connect("timeout",this,nameof(Attack));
        AddChild(t);*/
    }

    public override void _PhysicsProcess(float delta)
    {
        if(IsAttacking) return;
        if(IsDead) return;
        if(IsMovingToAttack){
            GetParent<KinematicBody>().LookAt(Target.GlobalTranslation,Vector3.Up);
            GetParent<KinematicBody>().MoveAndSlide(GlobalTranslation.DirectionTo(Target.GlobalTranslation));
            DistanceToAttack =- delta;
            if( DistanceToAttack < 0 ){
                IsMovingToAttack = false;
            }
            return;
        }
        if(MoveRandom)
            if( RandomMoveAngle > 0){
                RandomMoveAngle -= delta * TURN_SPEED;
                GetParent<Spatial>().RotateY(delta * TURN_SPEED);
            }else if( RandomMoveDistance > 0 ){
                if ( !GetNode<Area>("../../..").OverlapsBody(this) ) {
                    MoveRandom = false;
                    MoveGoBack = false;
                    RandomGoBack();
                    return;
                }
                GetParent<KinematicBody>().MoveAndSlide(Vector3.Forward.Rotated(Vector3.Up,GetParent<Spatial>().Rotation.y));
                RandomMoveDistance -= delta;
            }else{
                MoveRandom = false;
                RandomMove();
            }
        else if(MoveGoBack){
            if( RandomMoveDistance > 0 ){
                GetParent<KinematicBody>().MoveAndSlide(Vector3.Forward.Rotated(Vector3.Up,GetParent<Spatial>().Rotation.y));
                RandomMoveDistance -= delta;
            }else{
                MoveGoBack = false;
                RandomMove();
            }
        }
    }

    private void OnPLayerClose(Player p){
        OnHit(0);
    }

    private async void RandomMove(){
        //await ToSignal(GetTree().CreateTimer(random.Next(TimerToMoveMin,TimerToMoveMax)), "timeout");
        waiterTimer.WaitTime = random.Next(TimerToMoveMin,TimerToMoveMax);
        waiterTimer.Start();
        await ToSignal(waiterTimer, "timeout");
        
        RandomMoveAngle = (float)GD.RandRange( 0 , Math.PI );
        RandomMoveDistance = (float)GD.RandRange( MoveMin , MoveMax );
        MoveRandom = true;
    }

    private async void RandomGoBack(){
        
        //TODO start timer in one methode
        waiterTimer.WaitTime = random.Next(TimerToMoveMin,TimerToMoveMax);
        waiterTimer.Start();
        await ToSignal(waiterTimer, "timeout");

        RandomMoveAngle = 2;
        GetParent<Spatial>().LookAt(GetNode<Spatial>("../../..").GlobalTranslation,Vector3.Up);
        MoveGoBack = true;
    }

    public async void OnHit(int damage){

        if(IsDead)    return;

        if(!IsAggro){
            RightEye.SetSurfaceMaterial(0,AggroColorEyes);
            LeftEye.SetSurfaceMaterial(0,AggroColorEyes);
            playBack.Travel("Aggro");

            Timer timer = new Timer();
            timer.WaitTime = 8;
            timer.Autostart = true;
            timer.Connect("timeout",this,nameof(Attack));
            AddChild(timer);
        }
        IsAggro = true;



        Target = this.GetServiceFromIOC<PlayerData>().GetPlayerPosition();
        

        HP -= damage ;
        if ( HP > 0 ){
            lifeDisplay.Value = HP;
            return;
        }
        
        IsDead = true;
        lifeDisplay.QueueFree();
        float t = GetNode<AnimationPlayer>("AnimationPlayer").GetAnimation("Die").Length;
        playBack.Travel("Die");
        await ToSignal(GetTree().CreateTimer(t), "timeout");
        GetParent().QueueFree();

    }

    private void Attack(){

        if( Target == null) return;
        float targetDistance = Target.GlobalTranslation.DistanceTo(GlobalTranslation);

        if( targetDistance > 2 ){
            IsMovingToAttack = true;
            DistanceToAttack = targetDistance - 1;

            return;
        }
        IsMovingToAttack = false;
        IsAttacking = true;
        float t = GetNode<AnimationPlayer>("AnimationPlayer").GetAnimation("Attack").Length;
        playBack.Travel("Attack");
        /*Area a = AttackScene.Instance<Area>();
        GetParent().AddChild(a);
        await ToSignal(GetTree().CreateTimer(t), "timeout");
        if(!IsDead)
            foreach( var b in a.GetOverlappingBodies()  ){
                if(b is Player p){
                    p.OnHit(75);
                }
            }
        IsAttacking = false;
        a.QueueFree();*/

    }
}
