using System.Collections.Generic;
using Godot;

public class MobAttackSceneZone : Area,IMobAttckScene
{		
    public int DamageMax {get;set;} = 2;
    public float Radius {get;set;} = 2;
    public float Height {get;set;} = 2;

    public override void _Ready()
    {
        CollisionShape cs = GetNode<CollisionShape>("CollisionShape");
        CylinderShape shape = new CylinderShape();

        MeshInstance mi = GetNode<MeshInstance>("MeshInstance");
        CylinderMesh m = new CylinderMesh();
        m.TopRadius = Radius;
        m.BottomRadius = Radius;
        m.Height = Height;
        
        SpatialMaterial mat = new SpatialMaterial();
        mat.AlbedoColor = new Color(1, 0, 0, 0.3f);
        mat.FlagsTransparent = true;
        mat.ParamsCullMode = SpatialMaterial.CullMode.Disabled;
        m.SurfaceSetMaterial(0,mat);
        mi.Mesh = m;


        shape.Radius = Radius;
        shape.Height = Height;

        cs.Shape = shape;
        
    }

    /*
    public int CalculDamage(Player[] targets){return 0;}
    public bool IsReachable(float distance){
        return distance < Radius - 1;
    }

    public void Stop(){
        QueueFree();
    }
    
    public void Start(){}
    */

    public void Hit(){
        Godot.Collections.Array l = GetOverlappingBodies();
        foreach( PhysicsBody b in l ){
            GD.Print( " =>  " );
            GD.Print( b.Name );
            if(b is Player p){
                p.OnHit(DamageMax);
            }
        }
        //List<PhysicsBody> l = GetOverlappingBodies();
    }
    

}























































