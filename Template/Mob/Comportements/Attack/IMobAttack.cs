using System;
public interface IMobAttack{

    void AddFinishAction(Action finish);
    void removeFinishAction(Action finish);
    void Start(MobBase parent);

    void Reset();

}