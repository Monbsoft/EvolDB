using System.IO;

namespace Monbsoft.EvolDB.Commits
{
    public interface ICommitBuilder
    {
        Models.Commit Build(FileInfo file);
    }
}