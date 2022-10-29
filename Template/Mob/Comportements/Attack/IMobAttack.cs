using System;
public interface IMobAttack{

    int DamageMax {get;}
    void AddFinishAction(Action finish);
    void removeFinishAction(Action finish);
    void Start(MobBase parent,Player target);

    void AttackAction(float delta);
    void Reset();
    float GetDistMinToAttackTarget();

}