using MVC.Abstractions.Services.AD;
using MVC.Services.Implementations.AD;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace MVC.Infrastructure.ContainerConfigs
{
    public static class ApplicationServicesInstaller
    {
        public static void ConfigureApplicationServices()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserService, UserService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
