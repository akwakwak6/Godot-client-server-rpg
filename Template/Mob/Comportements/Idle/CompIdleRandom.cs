using System;
using Godot;

public class CompIdleRandom : IMobCompIdle{

    public int MoveMin {get; set;} = 1;
    public int MoveMax {get; set;} = 5;
    public int TimerToMoveMin {get; set;} = 1000;
    public int TimerToMoveMax {get; set;} = 10000;
    public float Speed {get; set;} = 2;
    public float TurnSpeed {get; set;} = 2;

    private readonly Random random = new Random();
    private readonly KinematicBody parent;
    private Spatial Zone;

    private float distToDo = -1f,rotToDo = -1f;
    private bool isWaiting = false;
    private bool outOfZone = false;
    private Vector3 Velo = Vector3.Zero;

    private readonly Delay Delay = new Delay();

    public CompIdleRandom(KinematicBody _parent){
        parent = _parent;
    }

    public void IdleAction(float delta){

        Vector3 vl = Velo * Vector3.Up;
        vl.y -= 9.8f;

        if(!isWaiting){//TODO can move only clockwise => change to do both way
            if(outOfZone){
                parent.LookAt(Zone.GlobalTranslation,Vector3.Up);
                vl += Speed  * Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y);
            }else if( rotToDo > 0){
                rotToDo -= delta * TurnSpeed;
                parent.RotateY(delta * TurnSpeed);
            }else if( distToDo > 0 ){
                vl += Speed  * Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y);
                distToDo -= delta * Speed;
            }else{
                RandomMove();
            }
        }

        Velo = parent.MoveAndSlide(vl,Vector3.Up);

    }

    public void OnOutOfIdleZone(Spatial zone){
        Delay.Cancel();
        Zone = zone;
        outOfZone = true;
        isWaiting = false;
    }

    public void OnEnterInIdleZone(Spatial zone){
        Zone = zone;
        outOfZone = false;
        isWaiting = false;
    }

    private async void RandomMove(){
        isWaiting = true;
        await Delay.wait(random.Next(TimerToMoveMin,TimerToMoveMax));
        isWaiting = false;
        rotToDo = (float)GD.RandRange( 0 , Math.PI );
        distToDo = (float)GD.RandRange( MoveMin , MoveMax );
    }

    public void Reset(){
        Delay.Cancel();
        Velo = Vector3.Zero;
        distToDo = -1f;
        rotToDo = -1f;
        isWaiting = false;
    }
}