using m = Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commit
{
    public interface IMigrationParser
    {
        m.Commit Parse(string name);
    }
}