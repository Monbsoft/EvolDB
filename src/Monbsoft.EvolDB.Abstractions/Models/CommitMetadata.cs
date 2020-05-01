using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public class CommitMetadata
    {
        public string Reference { get; set; }

        public string Message { get; set; }

        public string Hash { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
