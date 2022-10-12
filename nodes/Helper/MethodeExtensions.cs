using Microsoft.Extensions.DependencyInjection;

public static class MethodeExtensions
{
    public static T GetServiceFromIOC<T>(this System.Object o){
        return IocContainer.ServiceProvider.GetService<T>();
    }
}



