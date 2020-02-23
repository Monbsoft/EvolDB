using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commit
{
    public interface IMigrationParser
    {
        Token Parse(string name);
    }
}