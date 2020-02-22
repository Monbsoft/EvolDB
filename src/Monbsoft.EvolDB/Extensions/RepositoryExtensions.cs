using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repository;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.IO;

namespace Monbsoft.EvolDB.Extensions
{
    public static class RepositoryExtensions
    {
        public static CommandLineBuilder ConfigureRepository(this CommandLineBuilder builder)
        {
            builder.UseMiddleware((context) =>
            {
                var repository = new RepositoryBuilder()
                    .Build();
                context.BindingContext.AddService(typeof(IRepository), () => repository);
            });

            return builder;
        }
    }
}
