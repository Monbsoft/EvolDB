using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.Extensions.FileProviders
{
    public class PhysicaFileService : IFileService
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
