using System.Collections.Generic;
using System.IO;

namespace Monbsoft.Extensions.FileProviders
{
    public class PhysicalFileService : IFileService
    {
        public IDirectoryInfo CreateFolder(string path)
        {
            return new PhysicalDirectoryInfo(path);
        }

        public IFileInfo CreateFile(string path)
        {
            return new PhysicalFileInfo(path);
        }


    }
}