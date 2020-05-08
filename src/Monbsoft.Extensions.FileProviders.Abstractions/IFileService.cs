
namespace Monbsoft.Extensions.FileProviders
{
    public interface IFileService
    {
        IFileInfo GetFile(string path);
        IDirectoryInfo GetFolder(string path);
    }
}