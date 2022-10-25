using System;
public interface IMobAttack{

    void AddFinishAction(Action finish);
    void removeFinishAction(Action finish);
    void Start(MobBase parent,Player target);
    void Reset();
    float GetDistMinToAttackTarget();

}