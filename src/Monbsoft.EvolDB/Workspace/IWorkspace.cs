using Microsoft.Extensions.Configuration;

namespace Monbsoft.EvolDB.Workspace
{
    public interface IWorkspace
    {
        IConfigurationRoot Configuration { get; }
        string Name { get;  }
        void Create();
    }
}