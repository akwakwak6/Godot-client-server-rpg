using Godot;

public interface IMobAttckAnime: IMobAttack{
    
    Area HitBoxArea {get;set;}
    int DamageMax{get;set;}
    int Time{get;set;}

}