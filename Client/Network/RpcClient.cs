using System.Threading.Tasks;
using Godot;

public class Tp : ConvertGodoData{
    public string para {get;set;}
}

public class RpcClient : IrpcClient{

    //public IrpcFunction Rpc{get;}
    private IClientNetwork ClientNetwork;

    public RpcClient(){
        ClientNetwork = this.GetServiceFromIOC<IClientNetwork>();
    }

    public async Task<string> GetHelloPara(string para){
        Tp t2 = await ClientNetwork.Request<Tp>(new Tp(){para = para});
        GD.Print(t2.para);
        Tp t = await ClientNetwork.Request<Tp>();
        return t.para;

    }
}