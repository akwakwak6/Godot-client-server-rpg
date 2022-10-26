using System.Collections.Generic;
using Godot;
public interface IMob{

    public enum States{Idle,Aggro,Dead};

    public States State {get;}
    // public void ConnectIdie(Action<IMob> fct);
    // public void DisconnectIdie(Action<IMob> fct);
    public void OnOutOfIdleZone(Spatial zone);
    public void OnEnterIdleZone(Spatial zone);
    public void OnOutOfAgrroZone(Spatial zone);
    public void OnHit(int damage,Player p);
    //public List<IPlayer> getTargets();
    //public List<IAttack> getAttacks();
    //public void startAnnimation(string name);
    //public List<Attack> getAttack();

    public MobTargets GetTargets();
    public List<Player> GetTargetList();

}