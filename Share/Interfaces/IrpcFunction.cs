using System.Reflection.Emit;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Godot;
using System.Reflection;

public interface IrpcFunction{
    public Tp GetHelloPara(Tp s);
    public PlayerModel GetUserTime(PlayerModel p);
}
public interface IrpcClient : IrpcFunction{
    //public IrpcFunction Rpc{get;}   
}

public interface IrpcServer {
    //public List<object> methodes {get;}
    public ConvertGodoData Call(int cmd,object[] paras); 
}

public interface IrpcClientServer{
    public int getIndex(string mthName);
}

public class test : IrpcServer,IrpcClientServer{

    private List<MethodInfo> methodes {get; set;} = new List<MethodInfo>(); 
    public test(){
        Type intType = typeof(IrpcFunction);
        Type t2 = typeof(test);
        var lm = t2.GetMethods();
        foreach(var m in intType.GetMethods()){
            MethodInfo[] list = lm.Where( mm => mm.Name == m.Name).ToArray();
            if( list.Length != 1 ) {
                if( list.Length == 0 ) throw new Exception(" no methode for "+m.Name);
                else throw new Exception(" multi methodes "+m.Name+". overide not implemented yet");
            }
            methodes.Add(list.First());
        }
    } 

    public ConvertGodoData Call(int cmd,object[] paras){
        GD.Print(methodes.Count);
        GD.Print(cmd);
        GD.Print(methodes[cmd].Name);
        return (ConvertGodoData) methodes[cmd].Invoke(this,new object[]{paras});
        //new object[] {methodes[cmd].Invoke(this,paras)};
    }

    public Tp GetHelloPara(object[] para){//TODO May be I can put private and get as well in getMethdes
        Tp tt = para.GetModelData<Tp>();
        tt.para += " :-) 😴" ;
        return tt;
    }

    public ConvertGodoData GetUserTime(object[] pt){//TODO May be I can put private and get as well in getMethdes

        PlayerModel p = pt.GetModelData<PlayerModel>();
        GD.Print("paras");
        GD.Print(p.Time);
        //PlayerModel p = paras.GetModelData<PlayerModel>();
        
        p.Time = 666;
        return p;
    }

    public int getIndex(string mthName){
        return methodes.FindIndex(m => m.Name == mthName);
    }
}