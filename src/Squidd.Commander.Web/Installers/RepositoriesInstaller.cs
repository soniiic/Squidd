using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Squidd.Commander.DAL;

namespace Squidd.Commander.Web.Installers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<SqliteBaseRepository>()
                .BasedOn<SqliteBaseRepository>()
                .WithServiceAllInterfaces()
                .Configure(c => c.DependsOn(Dependency.OnValue("baseDirectory", HttpRuntime.BinDirectory)))
                .LifestylePerWebRequest());
        }
    }
}