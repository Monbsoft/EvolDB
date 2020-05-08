using System.Collections.Generic;
using System.IO;

namespace Monbsoft.Extensions.FileProviders
{
    public class PhysicalFileService : IFileService
    {
        public IDirectoryInfo GetFolder(string path)
        {
            return new PhysicalDirectoryInfo(path);
        }

        public IFileInfo GetFile(string path)
        {
            return new PhysicalFileInfo(path);
        }


    }
}