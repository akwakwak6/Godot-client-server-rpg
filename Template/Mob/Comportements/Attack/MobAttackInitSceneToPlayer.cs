using Godot;

public class MobAttackInitSceneToPlayer<S> : MobAttackInitScene<S> where S : Spatial,IMobAttackScene{

    public MobAttackInitSceneToPlayer(MobAttackSceneParaBase paras):base(paras){}

    public override async void Start(MobBase parent,Player target){
        S n =  SceneResource.Instance<S>();
        n.setPara(Paras);  
        n.Visible = false;
        parent.GetTree().Root.AddChild(n);
        parent.PlayAnimation(Paras.Name);
        await Delay.wait(Paras.TimeToHit);
        n.GlobalTransform = parent.GetWheapon().GlobalTransform;
        n.LookAt(target.GlobalTranslation + (Vector3.Up * 0.5f),Vector3.Up);
        n.Visible = true;
        n.Hit();
        await Delay.wait(Paras.Time - Paras.TimeToHit);
        n.QueueFree();
        Finish?.Invoke();
    }


}