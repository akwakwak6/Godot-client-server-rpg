using System;
using Godot;
public class PlayerData{

    private Camera Camera;
    private Action<Camera> CameraUpdate;

    public Camera ListenCameraUpdate(Action<Camera> a){
        CameraUpdate+=a;
        return Camera;
    }

    public void RemoveCameraUpdate(Action<Camera> a){
        CameraUpdate-=a;
    }


    public void SetPlayer(Camera c){
        Camera = c;
        CameraUpdate?.Invoke(c);
    }

}