using States = IMob.States;
using Godot;

public class AggroZone : Area
{

    [Export]
    private readonly float Radius = 1;
    [Export]
    private readonly float FrontShift = 0.8f;
    [Export]
    private readonly bool ShareTargets = true;

    private IMob mob;

    public override void _Ready()
    {
        mob = GetParent<IMob>();
        CollisionShape cs = GetNode<CollisionShape>("CollisionShape");
        cs.Shape = new CylinderShape(){
            Height = 2,
            Radius = Radius
        };
        Vector3 t =  cs.Translation;
        t.z = - FrontShift;
        cs.Translation = t;
        Connect("body_entered",this,nameof(chackBody));
    }

    private void chackBody(Node n){
        if(n is Player p)
            mob.OnHit(0,p);
        
        if(!ShareTargets)   return;
        if(n is IMob m){
            if(m.State != States.Aggro) return;
            var e = m.getTargets().GetEnumerator();
            e.MoveNext();
            mob.OnHit(0,e.Current.Key);
        }
    }


}
