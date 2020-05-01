using Monbsoft.EvolDB.Models;
using System.Collections.Generic;

namespace Monbsoft.EvolDB.Services
{
    public interface IDifferenceService
    {
        List<DiffResult> Compare(List<CommitMetadata> metadata);
    }
}