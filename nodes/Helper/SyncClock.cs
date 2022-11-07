
using Godot;

public class SyncClock{

    private static SyncClock instance;

    public ulong ServerTime {get;private set;} = 0;

    private SyncClock(){
        GD.Print("init sync clock");
    }

    public static SyncClock GetInstance(){
        return instance ?? new SyncClock();
    }

    public void InitSync(){
        
    }

}