using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public interface IMigrationParser
    {
        bool TryParse(string name, out Commit commit);
    }
}