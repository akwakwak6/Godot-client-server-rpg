//using Microsoft.Extensions.DependencyInjection.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using Godot;
public class IocContainer : Node{

    private readonly ServiceCollection Collection;
    public static ServiceProvider ServiceProvider {get; private set;}

    public IocContainer(){
        Collection = new  ServiceCollection();
    }

    private Network network;

    public override void _Ready(){
        
        if(ServiceProvider != null) return;

        RegisterService();
        ServiceProvider = Collection.BuildServiceProvider();

                //TODO remove
        RpcClient rc = new RpcClient();
    }

    private void RegisterService(){



        Network n = GetNode<Network>("/root/Network");
        WorldManager wn = GetNode<WorldManager>("/root/WorldManager");
        Collection.AddSingleton<IClientNetwork>(n);
        Collection.AddSingleton<IServerNetwork>(n);
        Collection.AddSingleton<WorldManager>(wn);
        Collection.AddSingleton<PlayerData>();
        //Collection.AddSingleton<IrpcClient,RpcClient>();
        Collection.AddSingleton<IrpcServer,test>();
        Collection.AddSingleton<IrpcClientServer,test>();


    }
}


