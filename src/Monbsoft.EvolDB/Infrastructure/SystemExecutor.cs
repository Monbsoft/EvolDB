using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Infrastructure
{
    internal class SystemExecutor
    {
        private const string SystemCommandName = "dotnet";
        private readonly ILogger<SystemExecutor> _logger;

        public SystemExecutor(ILogger<SystemExecutor> logger)
        {
            _logger = logger;
        }

        public void CreateProject()
        {
            
        }

        public Task<int> ExecuteAsync(string args)
        {            
            return Process.ExecuteAsync(
                SystemCommandName,                
                args,
                stdOut: line =>
                {
                    _logger.LogInformation(line);
                },
                stdErr: line =>
                {
                    _logger.LogError(line);
                });

        }

        private static FileInfo FindSystemPath()
        {
            FileInfo fileInfo = null;
            using (var process = System.Diagnostics.Process.Start(SystemCommandName))
            {
                if(process != null)
                {
                    fileInfo = new FileInfo(process.MainModule.FileName);
                }
            }
            return fileInfo;
        }

    }
}
