using System.Linq;
using System.Collections.Generic;
public class MobTargets{

    public int Count { get{return Targets.Count;}}

    private Dictionary<Player,TargetData> Targets = new Dictionary<Player,TargetData>();
    private MobBase Mob;

    private bool FlagDistance = false;

    public MobTargets(MobBase mob){
        Mob = mob;
    }

    public void PlayerHit(Player target,int damage){
        if(Targets.ContainsKey(target)){
            Targets[target].Damage += damage;
        }else{
            Targets.Add(target,new TargetData(damage));
        }
    }

    public List<Player> GetList(){
        return Targets.Select(p => p.Key).ToList();
    }

    public void Reset(){//TODO call this methode at the bgining or in the end
        FlagDistance = false;
        foreach( var kv in Targets ){
            kv.Value.Reset();
            //TODO remove if Dead
        }
    }

    public float GetSmallestDistance(){
        if(!FlagDistance) CalculTargetDistance();
        return Targets.Select( kv => kv.Value.Distance ).Min( d => (float)d );
    }


    private void CalculTargetDistance(){
        FlagDistance = true;
        foreach(var kv in Targets){
            kv.Value.Distance = kv.Key.GlobalTranslation.DistanceTo(Mob.GlobalTranslation);  
        }
    }

    

    class TargetData{
        public int Damage {get;set;}
        public float? Distance {get;set;} = null;
        public TargetData(int damage){
            Damage = damage;
        }
        public void Reset(){
            Distance =  null;
        } 
    };

}