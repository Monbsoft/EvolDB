using Monbsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monbsoft.EvolDB.Tests.Infrastructure
{
    public class TestDirectoryInfo : IDirectoryInfo
    {
        private List<IFileInfo> _files;
        private List<IDirectoryInfo> _subfolders;

        public TestDirectoryInfo()
        {
            _files = new List<IFileInfo>();
            _subfolders = new List<IDirectoryInfo>();
        }

        public TestDirectoryInfo(string name)
            : this()
        {
            Name = name;
        }

        public bool Exists { get; set; }

        public string Name { get; set; }

        public string PhysicalPath { get; set; }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFile(string name)
        {
            throw new NotImplementedException();
        }
        public IList<IFileInfo> GetFiles()
        {
            return _files;
        }

        public IDirectoryInfo GetFolder(string name)
        {
            return _subfolders.FirstOrDefault(d => d.Name == name);
        }

        public TestDirectoryInfo WithDirectory(TestDirectoryInfo subdirectory)
        {
            _subfolders.Add(subdirectory);
            return this;
        }
        public TestDirectoryInfo WithFile(TestFileInfo file)
        {
            _files.Add(file);
            return this;
        }
    }
}
