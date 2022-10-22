using System;
using Godot;

public class MobAttack{

    public Action Finish;

    private MobAttackZone Attack;

    public MobAttack(MobAttackZone attack){
        Attack = attack;
    }

    public void Start(Node n){
        Attack.Finish += Finish;
        Attack.Start(n);   
    }

}