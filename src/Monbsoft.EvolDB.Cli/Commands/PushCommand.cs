using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class PushCommand
    {


        public PushCommand()
        {

        }

        public static Command Create()
        {
            var command = new Command(
                name: "push",
                description: "Update remote commits using local commits");

            command.Handler = CommandHandler.Create<IHost>((host) =>
            {
                var pushCommand = new PushCommand();

            });

            return command;
        }
    }
}
