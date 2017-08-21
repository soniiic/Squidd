using System.Collections.Generic;
using Squidd.Commander.Domain.Entities;

namespace Squidd.Commander.Domain.Repositories
{
    public interface IRunnerRepository
    {
        List<Runner> GetAll();
    }
}