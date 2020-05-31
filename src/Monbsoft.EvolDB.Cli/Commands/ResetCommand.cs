using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monbsoft.EvolDB.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class ResetCommand
    {
        private readonly ICommitService _commitService;

        public ResetCommand(ICommitService commitService)
        {
            _commitService = commitService;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "reset",
                description: "Reset the current commit to the specified state.");

            command.Handler = CommandHandler.Create<IHost>(async host =>
            {
                var commitService = host.Services.GetRequiredService<ICommitService>();
                var resetCommand = new ResetCommand(commitService);
                await resetCommand.ExecuteAsync().ConfigureAwait(false);

            });
            return command;
        }

        public  Task ExecuteAsync()
        {
            return _commitService.ResetAsync();
        }
    }
}
