using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using System.Collections.Generic;
using System.IO;

namespace Monbsoft.EvolDB.Data
{
    public interface IRepository
    {
        public DirectoryInfo CommitFolder { get; set; }
        List<Commit> Commits { get; set; }
        IConfigurationRoot Configuration { get; }
        string Name { get; }

        bool Validate(Models.Commit commit);
    }
}