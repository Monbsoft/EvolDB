using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Services
{
    public interface ICommitService
    {
        void Create(string migration);

        void Execute(Commit commit);
    }
}