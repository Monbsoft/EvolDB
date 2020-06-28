using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public interface IReferenceParser
    {
        Commit CreateCommit(string reference);
    }
}