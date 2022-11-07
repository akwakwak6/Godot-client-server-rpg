using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using System;
using Godot;

public interface IrpcFunction{
    public Task<string> GetHelloPara(string para);
}

public interface IrpcServer : IrpcFunction{

    public List<object> methodes {get;} 
}

public class test{

    public List<object> methodes {get;private set;} = new List<object>(); 
    public test(){
        GD.Print("init test");
        Type intType = typeof(IrpcFunction);
        Type t2 = typeof(test);
        var lm = t2.GetMethods();
        foreach(var m in intType.GetMethods()){
            GD.Print(m.Name);
            var list = lm.Where( mm => mm.Name == m.Name).ToArray();
            GD.Print(list.Length);
            GD.Print(list[0].Name);
            GD.Print(list[0].Invoke(this,new object[]{"OKI good enought"}));
        }
        GD.Print("----------------");

    } 

    public string GetHelloPara(string para){
        return para+" :-) 😴";
    }
}

public interface IrpcClient : IrpcFunction{
    //public IrpcFunction Rpc{get;}   
}