using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public class Token
    {
        public Migration Migration { get; set; }

        public CommitVersion Version { get; set; }

        public string Message { get; set; }
    }
}
