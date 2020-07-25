using Microsoft.Extensions.Configuration;

namespace Monbsoft.EvolDB.SQLite
{
    public class SQLiteConfig
    {
        public const string SQLITE_CONNECTIONSTRING = "SQLITE_CONNECTIONSTRING";

        public SQLiteConfig(IConfigurationRoot configuration)
        {
            ConnectionString = configuration.GetValue<string>(SQLITE_CONNECTIONSTRING);
        }

        public string ConnectionString { get; set; }
    }
}