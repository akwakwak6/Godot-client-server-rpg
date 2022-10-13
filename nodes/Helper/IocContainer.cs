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
    }

    private void RegisterService(){

        Network n = GetNode<Network>("/root/Network");
        Collection.AddSingleton<IClientNetwork>(n);
        Collection.AddSingleton<IServerNetwork>(n);
        
        Collection.AddSingleton<PlayerData>();

    }
}


