using Godot;
public interface IMob{

    public  void OnOutOfIdleZone(Spatial zone);
    public void OnOutOfAgrroZone(Spatial zone);
    public void OnHit(int damage);//TODO Maybe add PLayer p?
    public bool IsAggro();

}