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

        public override bool AggroAction(Dictionary<Player,int> targets,float delta){
        
                //TODO maybe put back this instruction if error 
                //if(targets.Count == 0)  return false;
                if(IsAttacking) return false;

                Vector3 vl = Velo * Vector3.Up;
                vl.y -= 9.8f;

                var e = targets.GetEnumerator();
                e.MoveNext();
                float distanceTarget = e.Current.Key.GlobalTranslation.DistanceTo(parent.GlobalTranslation);

                if( distanceTarget < 1 ){//TODO distance target fct attack
                        IsAttacking = true;
                        AttackIndex = random.Next(Attacks.Count);
                        Attacks[AttackIndex].AddFinishAction(AttackFinish);
                        Attacks[AttackIndex].Start(Parent);
                        Parent.PlayAnimation("Attack1");
                        return false;
                }
                
                if(DistToSwitchIdle < distanceTarget)   return true;

                Vector2 diff = new Vector2(parent.GlobalTranslation.x - e.Current.Key.GlobalTranslation.x ,  parent.GlobalTranslation.z - e.Current.Key.GlobalTranslation.z   );
                float a = Mathf.Atan2(diff.x,diff.y); 
                parent.RotateY(  a - parent.Rotation.y );

                if(TargetDistance < distanceTarget){
                        vl += Vector3.Forward.Rotated(Vector3.Up,parent.Rotation.y)* Speed;
                } 

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