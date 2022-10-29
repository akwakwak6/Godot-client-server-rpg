using System;
using Godot;
public class PlayerPositionModel : ConvertGodoData{

    //TODO model are useless cause can not use with godot rpc => use g_rpc of c# 
    public Transform P {get;set;}
    public long T {get;set;}

}