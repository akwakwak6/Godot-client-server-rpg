using Godot;
using System;


//TODO interface healBar => SetValue
public class HealtBar : Spatial
{
    private MeshInstance Life;
    private Camera Camera;
    private PlayerData _PlayerData;
    private int _Value = 100;

    public float Value{
        get{
            return _Value;
        }
        set{
            if ( value < 0 ) value = 0;
            if ( value > 1 ) value = 1;
            Life.Scale = new Vector3(1f,value,1f);
            Life.Translation = new Vector3( + (1-value) - (1-value)/2 ,0,0 );
        }
    }
    public override void _Ready()
    {
        PlayerData _PlayerData = this.GetServiceFromIOC<PlayerData>();
        Camera = _PlayerData.ListenCameraUpdate(CameraUpdate);
        Life = GetNode<MeshInstance>("FC");
    }

    private void CameraUpdate(Camera c){
        Camera = c;
    }

    public override void _PhysicsProcess(float delta)
    {
        if(Camera == null) return;
        LookAt(Camera.GlobalTransform.origin,Vector3.Up); 
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _PlayerData?.RemoveCameraUpdate(CameraUpdate);
    }

}
