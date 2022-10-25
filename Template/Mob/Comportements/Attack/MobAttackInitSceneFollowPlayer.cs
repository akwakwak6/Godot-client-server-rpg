using Godot;

public class MobAttackInitSceneFollowPlayer<S> : MobAttackInitScene<S> where S : Spatial,IMobAttackScene{

    public MobAttackInitSceneFollowPlayer(MobAttackSceneParaBase paras):base(paras){}

    public override async void Start(MobBase parent,Player target){
        S n =  SceneResource.Instance<S>();
        n.setPara(Paras);
        target.AddChild(n);
        parent.PlayAnimation(Paras.Name);
        await Delay.wait(Paras.TimeToHit);
        n.Hit();
        n.QueueFree();
        await Delay.wait(Paras.Time - Paras.TimeToHit);
        Finish?.Invoke();
    }
}