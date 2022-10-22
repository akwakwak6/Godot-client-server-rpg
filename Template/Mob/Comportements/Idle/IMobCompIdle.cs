using Godot;

public interface IMobCompIdle{

    public void IdleAction(float delta);
    public void OnOutOfIdleZone(Spatial zone);
    public void OnEnterInIdleZone(Spatial zone);
    public void Reset();
}