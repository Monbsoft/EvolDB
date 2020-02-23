using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Monbsoft.EvolDB.Models
{
    public interface IRepository
    {
        IConfigurationRoot Configuration { get; }
        string Name { get;  }
        void Create();
        List<FileInfo> GetCommitFiles();
    }
}