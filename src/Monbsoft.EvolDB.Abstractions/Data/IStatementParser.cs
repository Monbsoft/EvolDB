using Monbsoft.EvolDB.Models;
using System.Collections.Generic;

namespace Monbsoft.EvolDB.Data
{
    public interface IStatementParser
    {
        IEnumerable<string> ParseQueries(string content);
    }
}