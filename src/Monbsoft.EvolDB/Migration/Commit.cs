using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Migration
{
    public class Commit
    {


        public string Hash { get; set; }

        public MigrationVersion Version { get; set; }
    }
}
