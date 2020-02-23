using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using m = Monbsoft.EvolDB.Models;


namespace Monbsoft.EvolDB.Commit
{
    public class CommitBuilder
    {
        private readonly IMigrationParser _parser;

        public CommitBuilder(IMigrationParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public m.Commit Build(FileInfo file)
        {
            if(file == null || !file.Exists )
            {
                throw new ArgumentNullException(nameof(file));
            }

            _parser.Parse(file.Name);

            return null;
           
        }


    }
}
