using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class PushCommand
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly Repository _repository;

        public PushCommand(IDatabaseGateway gateway, Repository repository)
        {
            _databaseGateway = gateway;
            _repository = repository;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "push",
                description: "Update remote commits using local commits");

            command.Handler = CommandHandler.Create<IHost>(async (host) =>
            {
                var gateway  = host.Services.GetRequiredService<IDatabaseGateway>();
                var repository = host.Services.GetRequiredService<Repository>();
                var pushCommand = new PushCommand(gateway, repository);
                await pushCommand.ExecuteAsync();

            });

            return command;
        }

        public async Task ExecuteAsync()
        {
            await _databaseGateway.OpenAsync();
            var commits = await _databaseGateway.GetCommitsAsync();
        }
    }
}
