using System;
using System.Linq;
using System.Collections.Generic;
using Godot;

public class WorldManager : Node{

    private const int _interpolationOffset = 200;

    private readonly List<WorldStateModel> Worlds = new List<WorldStateModel>();
    private readonly Delay loop = new Delay();
    private ulong LastWorldStatTime = 0;
    private readonly PackedScene PlayerSkin = GD.Load<PackedScene>("res://nodes/Player/PlayerSkin/PlayerSkin.tscn");

    private readonly SyncClock sync;
    public WorldManager(){
        loop.boucle(10,Loop);
        sync = SyncClock.GetInstance();
        
    }

    private void Loop(){
        //TODO Syncronise time
        if( Worlds.Count <= 1 ) return;
        ulong renderTime = (Time.GetTicksMsec() - _interpolationOffset);
        int cpt = 0;
        while( Worlds.Count > 2 && renderTime > Worlds[2].T){
			Worlds.RemoveAt(0);
            cpt++;
        }
        //GD.Print("remove "+cpt);

        if(Worlds.Count > 2){
            //GD.Print("move "+Worlds.Count);
            float interFactor = (renderTime - Worlds[1].T) /  (float)( Worlds[2].T - Worlds[1].T);
            foreach( PlayerServer ps in Worlds[2].PS ){
                if(  !Worlds[1].PS.Any(p => p.Id == ps.Id)  ) continue;
                if( GetNode("/root/Main/PlayersNode").HasNode( ps.Id.ToString() ) ){
                    
                    Transform lastPos = Worlds[1].PS.Find(p => p.Id == ps.Id).TR ;//TODO better way ?
                           
                    GetNode<KinematicBody>("/root/Main/PlayersNode/"+ps.Id).GlobalTransform = lastPos.InterpolateWith(ps.TR,interFactor);

                }else{
                    if( ps.Id == GetTree().GetNetworkUniqueId() )   continue;
                    KinematicBody newP = PlayerSkin.Instance<KinematicBody>();
                    newP.Name = ps.Id.ToString();
                    GetNode("/root/Main/PlayersNode").AddChild(newP,true);
                    newP.GlobalTransform = ps.TR;
                }

            }

        }


        /*
        //EXTRAPOLATION
        if world_stat_buffer.size() > 2:
            var interpolation_factor = float(render_time - world_stat_buffer[1]["T"]) / float( world_stat_buffer[2]["T"] - world_stat_buffer[1]["T"])
            for p in world_stat_buffer[2].keys():
                if str(p) == "T" :
                    continue
                if p == get_tree().get_network_unique_id():
                    continue
                if not world_stat_buffer[1].has(p):
                    continue
                if has_node(str(p)):
                    var new_position = lerp(world_stat_buffer[1][p]["P"],world_stat_buffer[2][p]["P"],interpolation_factor)
                    get_node(str(p)).move_player(new_position)
                else:
                    spawnPalyer(p,world_stat_buffer[2][p].P)

        */


        /*GD.Print("-------Loop--------");
        foreach(WorldStateModel w in Worlds){
            foreach( PlayerServer ps in w.PS ){
                GD.Print(ps.Id);
                GD.Print(ps.TR);
            }
        }*/

    }

    public void AddWorldState(WorldStateModel w){
        if( LastWorldStatTime < w.T ){
            LastWorldStatTime = w.T;
            Worlds.Add(w);
        }        
    }

}