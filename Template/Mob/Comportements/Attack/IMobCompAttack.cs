
public interface IMobAttack
{
    int DamageMax {get;set;}
    bool IsReachable(float distance);
    void Stop();
    void Start();
    void Hit();
    int CalculDamage(Player[] targets);
}

