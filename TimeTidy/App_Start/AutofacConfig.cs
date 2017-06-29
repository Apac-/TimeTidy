using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TimeTidy.Models;
using TimeTidy.Persistance;
using TimeTidy.Services;

namespace TimeTidy.App_Start
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserManagerService>().As<IApplicationUserManagerService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication);

            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterFilterProvider();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}