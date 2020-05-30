using Monbsoft.Extensions.FileProviders;

namespace Monbsoft.EvolDB.Services
{
    public interface IHashService
    {
        string ComputeHash(IFileInfo file);
    }
}