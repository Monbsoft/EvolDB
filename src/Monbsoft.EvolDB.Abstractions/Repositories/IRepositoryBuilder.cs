using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Repositories
{
    public interface IRepositoryBuilder
    {
        Repository Build();
    }
}