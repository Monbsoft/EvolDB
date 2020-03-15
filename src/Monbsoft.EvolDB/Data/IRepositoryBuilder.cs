using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Data
{
    public interface IRepositoryBuilder
    {
        IRepository Build();
    }
}