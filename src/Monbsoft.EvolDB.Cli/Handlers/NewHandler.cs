using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Handlers
{
    public static class NewHandler
    {

        public static void Execute(IWorkspace workspace)
        {
            workspace.Create();
        }
    }
}
