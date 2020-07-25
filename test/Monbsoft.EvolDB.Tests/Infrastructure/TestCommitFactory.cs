using Castle.Core.Logging;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Tests.Infrastructure
{
    public class TestCommitFactory : ICommitFactory
    {
        private readonly string _extension;
        public TestCommitFactory(string extension)
        {
            _extension = extension;
        }
        public ICommitBuilder CreateBuilder(Repository repository)
        {
            return new CommitBuilder(new ReferenceParser(_extension), new HashService(), NullLogger<CommitBuilder>.Instance);
        }

        public IReferenceParser CreateParser(Repository repository)
        {
            return new ReferenceParser(_extension);
        }
    }
}
