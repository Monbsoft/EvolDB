using Monbsoft.EvolDB.Data;
using Monbsoft.Extensions.FileProviders;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitRepositoryTests
    {

        public void Test()
        {
            var mockFolder = new Mock<IDirectoryInfo>();

            var repository = new CommitRepository(mockFolder.Object);

        }

        public void ValidateTest()
        {
        }
    }
}
