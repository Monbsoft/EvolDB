using Monbsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.EvolDB.Tests.Infrastructure
{
    public class TestFileService : IFileService
    {
        private TestFileInfo _file;
        private TestDirectoryInfo _folder;


        public IFileInfo GetFile(string path)
        {
            return _file;
        }

        public IDirectoryInfo GetFolder(string path)
        {
            return _folder;
        }

        public TestFileService WithFile(TestFileInfo file)
        {
            _file = file;
            return this;
        }

        public TestFileService WithFolder(TestDirectoryInfo folder)
        {
            _folder = folder;
            return this;
        }
    }
}
