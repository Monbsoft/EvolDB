using Monbsoft.EvolDB.Models;
using Monbsoft.Extensions.FileProviders;
using System.IO;

namespace Monbsoft.EvolDB.Commits
{
    public interface ICommitBuilder
    {
        Commit Build(IFileInfo file);
    }
}