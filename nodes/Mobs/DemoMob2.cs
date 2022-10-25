using Godot;
using System;

public class DemoMob2 : MobBase
{

    public override void _Ready()
    {
        base._Ready();
        CompIdle = new CompIdleRandom(this){
            Speed = 5,
            TurnSpeed = 5,
            TimerToMoveMax = 4000,
            TimerToMoveMin = 2000
        };

        MobAttckAnime sword = new MobAttckAnime(GetNode<Area>("Body/Area")){
            Name = "AttackSword",
            DistMinToAttackTarget = 0.75f,
            DamageMax = 75
        };

        compAggro = new CompAggroAttackTarget(this,sword){
            Speed = 5,
            DistToSwitchIdle = 20
        };
        
    }

}
