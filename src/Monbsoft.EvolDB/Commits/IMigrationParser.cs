using m = Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public interface IMigrationParser
    {
        m.Commit Parse(string name);
        string ParseName(string migration);
    }
}