using Monbsoft.EvolDB.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Services
{
    public interface ICommitService
    {
        void Create(string migration);
        Task<IEnumerable<TreeEntry>> GetTreeEntries();
        Task PushAsync();
        Task ResetAsync();
    }
}