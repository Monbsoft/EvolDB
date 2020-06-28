using System.IO;

namespace Monbsoft.EvolDB.Services
{
    public interface IHashService
    {
        string ComputeHash(FileInfo file);
    }
}