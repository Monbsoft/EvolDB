using Monbsoft.EvolDB.Cli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Handlers
{
    public static class NewHandler
    {

        public static void Execute()
        {
            var workspace = new Workspace();
            workspace.Create();
        }
    }
}
