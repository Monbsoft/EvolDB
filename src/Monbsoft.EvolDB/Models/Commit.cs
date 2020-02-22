using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Migration
{
    public class Commit
    {
        public string Message { get; set; }

        public string Hash { get; set; }

        public CommitVersion Version { get; set; }
    }
}
