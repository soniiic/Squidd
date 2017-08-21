using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Squidd.Commander.Domain.Services;

namespace Squidd.Commander.Web.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<RunnerService>()
                .Pick().If(f => f.Name.EndsWith("Service"))
                .WithServiceSelf()
                .LifestylePerWebRequest());
        }
    }
}