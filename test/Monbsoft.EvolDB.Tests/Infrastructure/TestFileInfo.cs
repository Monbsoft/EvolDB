using Monbsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace Monbsoft.EvolDB.Tests.Infrastructure
{
    public class TestFileInfo : IFileInfo
    {
        public TestFileInfo()
        {
        }

        public TestFileInfo(string name)
        {
            Name = name;
            Exists = true;
        }
        public bool Exists { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public FileStream Create()
        {
            throw new NotImplementedException();
        }

        public FileStream OpenRead()
        {
            throw new NotImplementedException();
        }

        public List<string> ReadLines()
        {
            throw new NotImplementedException();
        }
    }
}