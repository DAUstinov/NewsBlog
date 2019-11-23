using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using NewsBlog.Controllers;
using NewsBlog.Interfaces;
using NewsBlog.Models;
using NewsBlog.Repositories;

namespace NewsBlog
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IController, HomeController>("Home");
            container.RegisterType<DbContext, BlogContext>(new PerResolveLifetimeManager());
        
            return container;
        }
    }
}