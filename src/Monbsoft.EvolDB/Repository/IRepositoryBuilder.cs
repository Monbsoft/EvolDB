using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Repository
{
    public interface IRepositoryBuilder
    {
        IRepository Build();
    }
}