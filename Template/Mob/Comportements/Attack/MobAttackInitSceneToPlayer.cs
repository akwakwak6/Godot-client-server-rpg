using Godot;

public class MobAttackInitSceneToPlayer<S> : MobAttackInitScene<S> where S : Spatial,IMobAttackScene{

    public MobAttackInitSceneToPlayer(MobAttackSceneParaBase paras):base(paras){}

    public override async void Start(MobBase parent,Player target){
        S n =  SceneResource.Instance<S>();
        n.setPara(Paras);  
        parent.PlayAnimation(Paras.Name);
        n.SetAsToplevel(true);
        parent.GetWheapon().AddChild(n);
        n.LookAt(target.GlobalTranslation + (Vector3.Up * 0.5f),Vector3.Up);
        await Delay.wait(Paras.TimeToHit);
        n.Hit();
        await Delay.wait(Paras.Time - Paras.TimeToHit);
        Finish?.Invoke();
    }


}