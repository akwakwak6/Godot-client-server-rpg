using Godot;
using System;

public class MobV2 : KinematicBody
{
    private SpatialMaterial IdleColorEyes = new SpatialMaterial();
    private SpatialMaterial AggroColorEyes = new SpatialMaterial();
    private MeshInstance RightEye;
    private MeshInstance LeftEye;

    public override void _Ready()
    {
        RightEye = GetNode<MeshInstance>("Body/Eyes/RightEye");
        LeftEye = GetNode<MeshInstance>("Body/Eyes/LeftEye");
        IdleColorEyes.AlbedoColor = new Color(0,0,0);
        AggroColorEyes.AlbedoColor = new Color(1,0,0);
        RightEye.SetSurfaceMaterial(0,IdleColorEyes);
        LeftEye.SetSurfaceMaterial(0,IdleColorEyes);
    }

    public virtual void OnOutOfIdleZone(Spatial zone){

    }
    public virtual void OnOutOfAgrroZone(Spatial zone){

    }
    public virtual void OnHit(int damage){

    }
    public virtual bool IsAggro(){
        return false;
    }


}
