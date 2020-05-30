using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class ResetCommand
    {
        public static Command Create()
        {
            var command = new Command(
                name: "reset",
                description: "Reset the current commit to the specified state.");

            command.Handler = CommandHandler.Create<IHost>(host =>
            {
                var resetCommand = new ResetCommand();

            });
            return command;
        }
    }
}
