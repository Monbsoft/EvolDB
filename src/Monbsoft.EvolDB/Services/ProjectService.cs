using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Infrastructure;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Services
{
    public class ProjectService
    {
        private readonly SystemExecutor _executor;

        public ProjectService(ILoggerFactory loggerFactory)
        {
            _executor = new SystemExecutor(loggerFactory.CreateLogger<SystemExecutor>());
        }



        public void Create(Project project)
        {
            _executor.ExecuteAsync("new console");
        }
    }
}
