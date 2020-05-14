using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Services;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class CommitCommand
    {
        private readonly ILogger<CommitCommand> _logger;
        private readonly ICommitService _commitService;

        public CommitCommand(ICommitService commitService, ILogger<CommitCommand> logger)
        {
            _commitService = commitService;
            _logger = logger;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "commit",
                description: "Create a commit");

            command.AddArgument(new Argument<string>("migration"));

            command.Handler = CommandHandler.Create<string, IHost>((migration, host) =>
                {
                    var commitService = host.Services.GetRequiredService<ICommitService>();
                    var logger = host.Services.GetRequiredService<ILogger<CommitCommand>>();
                    var commitCommand = new CommitCommand(commitService, logger);
                    commitCommand.CreateCommit(migration);
                });

            return command;
        }

        public void CreateCommit(string migration)
        {
            _logger.LogDebug("Creating commit...");
            _commitService.Create(migration);
            _logger.LogInformation($"Commit {migration} is created.");
        }
    }
}