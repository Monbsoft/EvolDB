using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Monbsoft.EvolDB.Repository
{
    public interface IRepository
    {
        IConfigurationRoot Configuration { get; }
        string Name { get;  }
        void Create();
        void Load();
        List<FileInfo> GetScripts();
    }
}