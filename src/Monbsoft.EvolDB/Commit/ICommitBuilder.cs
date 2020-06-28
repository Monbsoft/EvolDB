using System.IO;

namespace Monbsoft.EvolDB.Commit
{
    public interface ICommitBuilder
    {
        Models.Commit Build(FileInfo file);
    }
}