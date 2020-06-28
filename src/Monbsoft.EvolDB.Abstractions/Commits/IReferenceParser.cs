using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public interface IReferenceParser
    {
        bool Parse(string reference, out Commit commit);
    }
}