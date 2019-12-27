using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Workspace;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.IO;

namespace Monbsoft.EvolDB.Extensions
{
    public static class WorkspaceExtensions
    {
        public static CommandLineBuilder ConfigureWorkspace(this CommandLineBuilder builder)
        {
            builder.UseMiddleware((context) =>
            {
                var workspace = new WorkspaceBuilder()
                    .Build();
                context.BindingContext.AddService(typeof(IWorkspace), () => workspace);
            });

            return builder;
        }
    }
}
