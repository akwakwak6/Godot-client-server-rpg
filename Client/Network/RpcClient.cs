using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using System;
using Godot;

public class Tp : ConvertGodoData{
    public string para {get;set;}
}

public class RpcClient {//: IrpcClient{

    //public IrpcFunction Rpc{get;}
    private IClientNetwork ClientNetwork;
    private IrpcClientServer RpcList;

    public RpcClient(){
        ClientNetwork = this.GetServiceFromIOC<IClientNetwork>();
        RpcList = this.GetServiceFromIOC<IrpcClientServer>();

        GD.Print("AssemblyName");

        AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
        AssemblyBuilder ab =
            AssemblyBuilder.DefineDynamicAssembly(
                aName,
                AssemblyBuilderAccess.Run);

        // The module name is usually the same as the assembly name.
        ModuleBuilder mb =
            ab.DefineDynamicModule(aName.Name);

        TypeBuilder tb = mb.DefineType(
            "MyDynamicType",
             TypeAttributes.Public);

        // Define default constructor
        Type[] parameterTypes = { typeof(int) };
        ConstructorBuilder ctor1 = tb.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            Type.EmptyTypes);

        ILGenerator ctor1IL = ctor1.GetILGenerator();
        ctor1IL.Emit(OpCodes.Ldarg_0);
        ctor1IL.Emit(OpCodes.Call,typeof(object).GetConstructor(Type.EmptyTypes));
        ctor1IL.Emit(OpCodes.Ret);


        MethodBuilder meth = tb.DefineMethod(
            "MyMethod",
            MethodAttributes.Public,
            typeof(Tp),
            new Type[] { typeof(Tp) });

        ILGenerator methIL = meth.GetILGenerator();


        methIL.Emit(OpCodes.Ldarg_0);
        //methIL.Emit(OpCodes.Ldc_I4,32);

        /*
        methIL.Emit(OpCodes.Ldstr,tptp.para);
        */

        /*
        ConstructorInfo ci = typeof(Tp).GetConstructor(System.Type.EmptyTypes);
        methIL.Emit(OpCodes.Newobj, ci);
        */
        methIL.Emit(OpCodes.Ldarg_1);

        /*


        */

        var mm = typeof(RpcClient).GetMethod("testtest");
        MethodInfo bound = mm.MakeGenericMethod(typeof(Tp));
        methIL.Emit(OpCodes.Call,bound);
        //methIL.Emit(OpCodes.Call,this.GetType().GetMethod("testtest",BindingFlags.NonPublic | BindingFlags.Instance));
        //methIL.Emit(OpCodes.Ldc_I4,118);
        
        //methIL.Emit(OpCodes.Ldarg_1);
        
        methIL.Emit(OpCodes.Ret);




        Type t = tb.CreateType();                
        MethodInfo mi = t.GetMethod("MyMethod");
        object o1 = Activator.CreateInstance(t);
        Tp tptp2 = new Tp(){para = "OKI good 2"};
        object[] arguments = { tptp2 };
        GD.Print( ((Tp)mi.Invoke(o1, arguments)).para );


        foreach(var m in typeof(IrpcFunction).GetMethods()){
            GD.Print( m.Name );
            GD.Print( m.ReturnParameter.ParameterType );
            foreach( var p in m.GetParameters() )
                GD.Print(p.ParameterType);
        }
        
    }

    private async Task<T> GeneMet<T,S>(S d,[CallerMemberName] string caller = null) 
                                                    where S: ConvertGodoData,new() 
                                                    where T: ConvertGodoData,new(){
        return await ClientNetwork.Request<T>(RpcList.getIndex(caller),d);
    }

    public T testtest<T>(T u)where T : Tp{
        GD.Print("IN the fcr from asm   "+u.para);
        u.para += " yep yep yep";
        return u;
    }

    /*public Tp GetHelloPara(Tp s){
        return GeneMet<Tp,Tp>(s);
    }

    public PlayerModel GetUserTime(PlayerModel s){
        return GeneMet<PlayerModel,PlayerModel>(s);
    }*/

}