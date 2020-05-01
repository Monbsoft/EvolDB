using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public class DiffResult
    {
        public DiffResult()
        {

        }

        public string Reference { get; set; }

        public string Hash { get; set; }

        public bool IsTarget { get; set; }

        public bool IsSource { get; set; }
    }
}
