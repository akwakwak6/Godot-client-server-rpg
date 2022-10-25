using Godot;

public class MobAttackSceneZonePara : MobAttackSceneParaBase{
    public float Radius {get;set;} = 2;
    public float Height {get;set;} = 2;
}

public class MobAttackSceneZone : Area,IMobAttackScene
{
    private MobAttackSceneZonePara p;
    public void setPara(MobAttackSceneParaBase paras){
        p = (MobAttackSceneZonePara) paras;
    }

    public override void _Ready()
    {
        CollisionShape cs = GetNode<CollisionShape>("CollisionShape");
        CylinderShape shape = new CylinderShape();

        MeshInstance mi = GetNode<MeshInstance>("MeshInstance");
        CylinderMesh m = new CylinderMesh();
        m.TopRadius = p.Radius;
        m.BottomRadius = p.Radius;
        m.Height = p.Height;
        
        SpatialMaterial mat = new SpatialMaterial();
        mat.AlbedoColor = new Color(1, 0, 0, 0.3f);
        mat.FlagsTransparent = true;
        mat.ParamsCullMode = SpatialMaterial.CullMode.Disabled;
        m.SurfaceSetMaterial(0,mat);
        mi.Mesh = m;

        shape.Radius = p.Radius;
        shape.Height = p.Height;

        cs.Shape = shape;
        
    }

    public void Hit(){
        Godot.Collections.Array l = GetOverlappingBodies();
        foreach( PhysicsBody b in l ){
            if(b is Player pl){
                pl.OnHit(p.DamageMax);
            }
        }
    }


}



















































