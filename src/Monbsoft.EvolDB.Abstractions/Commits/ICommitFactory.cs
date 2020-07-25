using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public interface ICommitFactory
    {
        ICommitBuilder CreateBuilder(Repository repository);
        IReferenceParser CreateParser(Repository repository);
    }
}