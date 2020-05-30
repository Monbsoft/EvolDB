using Monbsoft.EvolDB.Models;
using System.Collections.Generic;

namespace Monbsoft.EvolDB.Data
{
    public interface IQueryParser
    {
        List<QueryToken> Parse(List<string> lines);
    }
}