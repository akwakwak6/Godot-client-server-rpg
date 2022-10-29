using Godot;

public interface IMobGoBackIdle{
    public float DistanceMax {get;}
    public bool StayAggro(MobTargets targets);
}

public class MobGoBackIdle : IMobGoBackIdle{

    public float DistanceMax {get;set;} = 5;

    public bool StayAggro(MobTargets targets){
        if( targets.Count == 0 ) return false;
        if( targets.GetSmallestDistance() > DistanceMax)    return true;
         return true;
    }
}