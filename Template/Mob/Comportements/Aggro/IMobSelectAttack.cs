using System.Collections.Generic;
using System;
public interface IMobSelectAttack{
    public int SelectAttack(MobTargets targets,List<IMobAttack> attacks);
}

public class MobSelectRandomAttack : IMobSelectAttack{
    private Random random = new Random();
    public int SelectAttack(MobTargets targets,List<IMobAttack> attacks){
        
        return random.Next(attacks.Count);
    }
}