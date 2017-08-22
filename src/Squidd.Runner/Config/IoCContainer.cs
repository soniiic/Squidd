using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Squidd.Runner.Handlers;
using Squidd.Runner.Middleware;

namespace Squidd.Runner.Config
{
    public static class IoCContainer
    {
        internal static IWindsorContainer Container { get; } = new WindsorContainer();

        public static void Configure(IApplicationSettings applicationSettings)
        {
            Container.Register(Component.For<IApplicationSettings>().Instance(applicationSettings));

            Container.Register(Classes.FromThisAssembly()
                .IncludeNonPublicTypes()
                .BasedOn(typeof(IHandler))
                .WithServiceDefaultInterfaces());

            Container.Register(Classes.FromThisAssembly()
                .IncludeNonPublicTypes()
                .BasedOn(typeof(IMiddleware))
                .WithServiceDefaultInterfaces());
        }
    }
}
