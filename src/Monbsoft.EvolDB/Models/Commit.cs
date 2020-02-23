using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public class Commit
    {
        public string FullName { get; set; }

        public Migration Migration { get; set; }

        public string Message { get; set; }

        public string Hash { get; set; }

        public CommitVersion Version { get; set; }

    }
}
