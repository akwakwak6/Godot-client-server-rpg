using System.Collections.Generic;
using System.Linq;
using System;
public interface IMobSelectAttack{
    public int SelectAttack(MobTargets targets,List<IMobAttack> attacks);
}

//TODO change name
public class MobSelectAttack : IMobSelectAttack{
    public int SelectAttack(MobTargets targets,List<IMobAttack> attacks){

        return attacks.Select((a, i) => new { i, a })
            .Where( o => o.a.GetDistMinToAttackTarget() > targets.GetSmallestDistance() )
            .OrderByDescending(o => o.a.DamageMax )
            .First().i;
    }
}