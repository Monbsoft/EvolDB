using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Monbsoft.EvolDB.Models
{
    public interface IRepository
    {
        public DirectoryInfo CommitFolder { get; set; }
        List<Commit> Commits { get; set; }
        IConfigurationRoot Configuration { get; }
        string Name { get; }

        bool Validate(Commit commit);
    }
}