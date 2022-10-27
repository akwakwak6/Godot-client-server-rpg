using Godot;

public class MobAttackInitSceneOnPlayer<S> : MobAttackInitScene<S> where S : Spatial,IMobAttackScene{

    public MobAttackInitSceneOnPlayer(MobAttackSceneParaBase paras):base(paras){}

    public override async void Start(MobBase parent,Player target){
        S n =  SceneResource.Instance<S>();
        n.setPara(Paras);
        parent.GetTree().Root.AddChild(n);
        n.GlobalTranslation = target.GlobalTranslation;
        parent.PlayAnimation(Paras.Name);
        await Delay.wait(Paras.TimeToHit);
        n.Hit();
        n.QueueFree();
        await Delay.wait(Paras.Time - Paras.TimeToHit);
        Finish?.Invoke();
    }
}