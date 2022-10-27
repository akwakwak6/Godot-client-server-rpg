
public class MobAttackSceneParaBase{
    public virtual int DamageMax{get;set;} = 3;
    public virtual int TimeToHit{get;set;} = 2000;
    public virtual string Name {get;set;}
    public virtual int Time{get;set;} = 4000;
    public virtual float DistMinToAttackTarget{get;set;} = 1;
}