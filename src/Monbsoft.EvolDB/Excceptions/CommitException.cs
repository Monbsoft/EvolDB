using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Excceptions
{
    public class CommitException : Exception
    {
        public CommitException(string message) 
            : base(message)
        {
        }

        public CommitException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
