using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Services;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class InitCommand
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ILogger<InitCommand> _logger;

        public InitCommand(IRepositoryService repositoryService, ILogger<InitCommand> logger)
        {
            _repositoryService = repositoryService;
            _logger = logger;
        }

        public string Name { get; }

        public static Command Create()
        {
            var command = new Command(
                name: "init",
                description: "Create a database project");

            command.AddArgument(new Argument<string>("name"));

            command.Handler = CommandHandler.Create<string, IHost>((name, host) =>
            {
                var repositoryService = host.Services.GetRequiredService<IRepositoryService>();
                var logger = host.Services?.GetRequiredService<ILogger<InitCommand>>();
                var initCommand = new InitCommand(repositoryService, logger);
                initCommand.CreateRepository(name);
            });

            return command;
        }

        public void CreateRepository(string name)
        {
            _logger.LogDebug("Creating repository...");
            _repositoryService.Create(name);
            _logger.LogInformation($"Repository {name} is created.");
        }
    }
}