using Godot;
using System;

public class DemoMob : MobBase
{
    public override void _Ready(){
        base._Ready();
        CompIdle = new CompIdleRandom(this){
            Speed = 5,
            TurnSpeed = 5,
            TimerToMoveMax = 4000,
            TimerToMoveMin = 2000
        };

        MobAttackZone zone= new MobAttackZone();
        zone.DamageMax = 20;
        zone.Radius = 4;
        zone.Height = 5;
        

        compAggro = new CompAggroAttackTarget(this,zone){
            Speed = 5,
            DistToSwitchIdle = 20
        };

    }



}
