using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using TimeTidy.Services;
using TimeTidy.Controllers;

namespace TimeTidy
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDbContextService, DbContextService>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}