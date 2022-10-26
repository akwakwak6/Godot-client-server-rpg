using System.Linq;
using System.Collections.Generic;
public class MobTargets{

    class TargetData{
        public int Damage{get;set;}
        public TargetData(int damage){
            Damage = damage;
        }

        public void Reset(){

        }
        
    };

    public int Count { get{return Targets.Count;}}

    private Dictionary<Player,TargetData> Targets = new Dictionary<Player,TargetData>();

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

    public void Reset(){
        //TODO check check if remove aggro of player
    }

    

}