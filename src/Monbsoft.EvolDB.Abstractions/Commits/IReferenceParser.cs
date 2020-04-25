using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public interface IReferenceParser
    {
        bool TryParse(string reference, out Commit commit);
    }
}