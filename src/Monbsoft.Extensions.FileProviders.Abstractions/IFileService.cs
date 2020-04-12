
namespace Monbsoft.Extensions.FileProviders
{
    public interface IFileService
    {
        IFileInfo CreateFile(string path);
        IDirectoryInfo CreateFolder(string path);
    }
}