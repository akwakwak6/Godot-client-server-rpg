using Godot;
using System;

public class HealtBar : Spatial
{
    private MeshInstance Life;
    private Camera Camera;
    private int _Value = 100;

    public int Value{
        get{
            return _Value;
        }
        set{
            if ( value < 0 ) value = 0;
            if ( value > 100 ) value = 100;
            float v = value/100f;
            Life.Scale = new Vector3(1f,v,1f);
            Life.Translation = new Vector3( + (1-v) - (1-v)/2 ,0,0 );
        }
    }
    public override void _Ready()
    {
        GD.Print("ready");
        PlayerData pd = this.GetServiceFromIOC<PlayerData>();
        Camera = pd.ListenCameraUpdate(CameraUpdate);

        Life = GetNode<MeshInstance>("FC");
        //Camera = 
    }

    private void CameraUpdate(Camera c){
        Camera = c;
    }

    public override void _Process(float delta)
    {
        if(Camera == null) return;
        Vector3 camera_pos = Camera.GlobalTransform.origin;
        camera_pos.y = 0;
        LookAt(camera_pos,Vector3.Up);
        
    }

}
