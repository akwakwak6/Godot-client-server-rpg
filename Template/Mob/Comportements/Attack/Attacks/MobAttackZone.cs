
public class MobAttackZone : AbstMobAttack{

    public float Radius {get;set;} = 2;
    public float Height {get;set;} = 2;


    private const string ScenePath = "res://Template/Mob/Comportements/Attack/Attacks/MobAttackSceneZone.tscn";
    public MobAttackZone():base(ScenePath){}

    protected override void initScene(IMobAttckScene s){
        MobAttackSceneZone ls = (MobAttackSceneZone)s;
        ls.Radius = Radius;      
        ls.Height = Height;
    }
    
}