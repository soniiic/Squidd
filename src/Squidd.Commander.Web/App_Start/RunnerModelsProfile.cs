using AutoMapper;
using Squidd.Commander.Domain.Entities;
using Squidd.Commander.Web.Models.RunnerModels;

namespace Squidd.Commander.Web.App_Start
{
    public class RunnerModelsProfile : Profile
    {
        public RunnerModelsProfile() : base(typeof(RunnerModelsProfile).Name)
        {
            CreateMap<Runner, RunnerOutputModel>();
        }
    }
}