using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Migration
{
    public interface ICommitLoader
    {
        void Load(IRepository workspace);
    }
}