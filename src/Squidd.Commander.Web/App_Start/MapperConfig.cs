using AutoMapper;

namespace Squidd.Commander.Web.App_Start
{
    public class MapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new RunnerModelsProfile()));
        }
    }
}