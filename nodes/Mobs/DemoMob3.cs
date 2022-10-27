using Godot;
using System;

public class DemoMob3 : MobBase
{

    public override void _Ready()
    {
        base._Ready();

        CompIdle = new CompIdleRandom(this){
            Speed = 5,
            TurnSpeed = 5,
            TimerToMoveMin = 2000,
            TimerToMoveMax = 4000
        };

        MobAttackInitSceneToPlayer<MobAttackSceneThrow> zone = 
            new MobAttackInitSceneToPlayer<MobAttackSceneThrow>(
                new MobAttackSceneThrowPara(){
                    Speed = 2,
                    DamageMax = 22,
                    TimeToHit = 0,
                    Name = "Atk",
                    DistMinToAttackTarget = 5,
                    Time = 5000
                }
            );
        
        compAggro = new CompAggroAttackTarget(this,zone){
            Speed = 5,
            DistToSwitchIdle = 20
        };
    }

    public override Spatial GetWheapon(){
        return GetNode<Spatial>("Body/Arms/LeftArm/Wheapon");
    }

}
