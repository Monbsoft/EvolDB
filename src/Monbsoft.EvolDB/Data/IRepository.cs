using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using Monbsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace Monbsoft.EvolDB.Data
{
    public interface IRepository
    {
        public IDirectoryInfo CommitFolder { get; set; }
        List<Commit> Commits { get; set; }
        IConfigurationRoot Configuration { get; }
        string Name { get; }
        IFileInfo CreateCommitFile(string name);
        bool Validate(Commit commit);
    }
}