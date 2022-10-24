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

        MobAttackInitScene<MobAttackSceneZone> zone = 
            new MobAttackInitScene<MobAttackSceneZone>(
                new MobAttackSceneZonePara(){
                    Radius = 4,
                    Height = 4,
                    DamageMax = 20,
                    TimeToHit = 2000
                }
            );

        compAggro = new CompAggroAttackTarget(this,zone){
            Speed = 5,
            DistToSwitchIdle = 20
        };

    }



}
