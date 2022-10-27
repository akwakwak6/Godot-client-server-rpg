using System.Collections.Generic;
using System;
using Godot;

public class CompAggroAttackTarget: CompAggroGoTarget{

        private MobBase Parent;
        private Random random = new Random();
        protected List<IMobAttack> Attacks = new List<IMobAttack>();
        private bool IsAttacking = false;
        private int AttackIndex;

        public CompAggroAttackTarget(MobBase _parent,List<IMobAttack> attacks):base(_parent){
               Attacks = attacks;
               Parent = _parent;
        }
        public CompAggroAttackTarget(MobBase _parent,params IMobAttack[] attacks):base(_parent){
                foreach(IMobAttack a in attacks){
                        Attacks.Add(a);        
                }
                Parent = _parent;
        }

        //TODO set Parant IMOB => get Target and much more
        public override bool AggroAction(MobTargets targets,float delta){       

                if(IsAttacking) return false;
                targets.Reset();
                GoBackIdle.StayAggro(targets);
        
                //TODO maybe put back this instruction if error 
                //if(targets.Count == 0)  return false;

                //TODO -> make Attack MoveToTarget then use it
                

                Player p = SelectTarget.SelectTarget(targets,Attacks);
                Vector3 vl = Velo * Vector3.Up;
                vl.y -= 9.8f;
                Vector2 diff = new Vector2(parent.GlobalTranslation.x - p.GlobalTranslation.x ,  parent.GlobalTranslation.z - p.GlobalTranslation.z   );
                float a = Mathf.Atan2(diff.x,diff.y); 
                parent.RotateY(  a - parent.Rotation.y );

                
                
                //float distanceTarget =p.GlobalTranslation.DistanceTo(parent.GlobalTranslation);
                //TODO interface to Select target and Attack
                AttackIndex = SelectAttack.SelectAttack(targets,Attacks);//random.Next(Attacks.Count);
                //if( distanceTarget < Attacks[AttackIndex].GetDistMinToAttackTarget() ){
                if( AttackIndex != null ){
                        IsAttacking = true;
                        //AttackIndex = random.Next(Attacks.Count);
                        Attacks[AttackIndex].AddFinishAction(AttackFinish);
                        Attacks[AttackIndex].Start(Parent,p);
                        //return false;
                }else{
                        vl += Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y)* Speed;
                }
                
                //if(DistToSwitchIdle < distanceTarget)   return true;

                /*if(TargetDistance < distanceTarget){
                        vl += Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y)* Speed;
                } */
                

                Velo = parent.MoveAndSlide(vl,Vector3.Up);
                
                return false;
        }

        private void AttackFinish(){
                IsAttacking = false;
                Attacks[AttackIndex]?.removeFinishAction(AttackFinish);
        }

        public override void Reset(){
                base.Reset();
                Attacks[AttackIndex]?.Reset();
        }

}