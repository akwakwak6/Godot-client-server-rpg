using System.Collections.Generic;
using System;
public interface IMobSelectTarget{
    public Player SelectTarget(MobTargets targets,List<IMobAttack> attacks);
}

public class MobSelectRandomTarget : IMobSelectTarget{
    private Random random = new Random();
    public Player SelectTarget(MobTargets targets,List<IMobAttack> attacks){
        return targets.GetList()[ random.Next(targets.Count) ];
    }
}

public class MobSelectFirstTarget : IMobSelectTarget{
    private Random random = new Random();
    public Player SelectTarget(MobTargets targets,List<IMobAttack> attacks){
        return targets.GetList()[0];
    }
}