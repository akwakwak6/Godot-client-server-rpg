using System.Collections.Generic;
using Godot;

public class CompAggroAttackTarget: CompAggroGoTarget{

        private MobBase Parent;
        protected MobAttackZone Attacks;
        //protected List<IMobAttack> Attacks;
        private bool IsAttacking = false;

        public CompAggroAttackTarget(MobBase _parent,MobAttackZone attack):base(_parent){
               Attacks = attack;
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

                //Attacks[0].
                //GD.Print(Attacks[0].Damage);
                if( distanceTarget < 1 ){
                        IsAttacking = true;
                        Attacks.Finish += AttackFinish;
                        Attacks.Start(Parent);
                        Parent.PlayAnimation("Attack1");
                        return false;
                }
                

                //TODO check distance  parent(x,z) - target(x,y)
                //TODO stop try to forward if obstacle ( raycast ? )
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
        }

        public override void Reset(){
                base.Reset();
                Attacks.Reset();
        }

}