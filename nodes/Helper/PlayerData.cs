using System;
using Godot;
public class PlayerData{

    private Camera Camera;
    private Action<Camera> CameraUpdate;

    private Spatial PlayerPosition;

    public Camera ListenCameraUpdate(Action<Camera> a){
        CameraUpdate+=a;
        return Camera;
    }

    public void RemoveCameraUpdate(Action<Camera> a){
        CameraUpdate-=a;
    }

    public Spatial GetPlayerPosition(){
        return PlayerPosition;
    }


    public void SetPlayer(Camera c,Player p){
        Camera = c;
        CameraUpdate?.Invoke(c);
        PlayerPosition = (Spatial) p;
    }

}