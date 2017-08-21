using System.Collections.Generic;
using System.Linq;
using Dapper;
using Squidd.Commander.Domain.Entities;
using Squidd.Commander.Domain.Repositories;

namespace Squidd.Commander.DAL
{
    public class RunnerRepository : SqlLiteBaseRepository, IRunnerRepository
    {
        public RunnerRepository(string baseDirectory) : base(baseDirectory)
        {
        }

        public List<Runner> GetAll()
        {
            return Run(c => c.QueryAsync<Runner>("SELECT Id, EndPoint, ComputerName, FriendlyName FROM Runner")).Result.ToList();
        }
    }
}