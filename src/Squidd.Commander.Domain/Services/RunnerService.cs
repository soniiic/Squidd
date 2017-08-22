using System.Collections.Generic;
using Squidd.Commander.Domain.Entities;
using Squidd.Commander.Domain.Repositories;

namespace Squidd.Commander.Domain.Services
{
    public class RunnerService
    {
        private readonly IRunnerRepository repository;

        public RunnerService(IRunnerRepository repository)
        {
            this.repository = repository;
        }

        public List<Runner> GetAll()
        {
            return repository.GetAll();
        }
    }
}
