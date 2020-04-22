using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class StatusCommand
    {
        private readonly Repository _repository;
        private readonly IDatabaseGateway _gateway;
        public StatusCommand(IDatabaseGateway gateway, Repository repository)
        {
            _repository = repository;
            _gateway = gateway;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "status",
                description: "Show the migration commits between the local repository and the remote repository.");

            command.Handler = CommandHandler.Create<IHost>(async (host) =>
            {
                var repository = host.Services.GetRequiredService<Repository>();
                var gateway = host.Services.GetRequiredService<IDatabaseGateway>();
                var statusCommand = new StatusCommand(gateway, repository);
                await statusCommand.GetStatusAsync();

            });

            return command;
        }

        public async Task GetStatusAsync()
        {
            await _gateway.OpenAsync();
            var commits = await _gateway.GetCommitsAsync();
        }
    }
}