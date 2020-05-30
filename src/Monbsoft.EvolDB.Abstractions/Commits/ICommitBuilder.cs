using Monbsoft.EvolDB.Models;
using Monbsoft.Extensions.FileProviders;

namespace Monbsoft.EvolDB.Commits
{
    public interface ICommitBuilder
    {
        Commit Build(IFileInfo file);
    }
}