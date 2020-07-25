using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Couchbase;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Data
{
    public class GatewayFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Repository _repository;

        public GatewayFactory(Repository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _loggerFactory = loggerFactory;
        }

        public IDatabaseGateway CreateGateway()
        {

            if(string.IsNullOrEmpty(_repository.ConnectionType))
            {
                throw new InvalidOperationException("Connection type is not configured.");
            }

            IDatabaseGateway gateway;
            switch(_repository.ConnectionType)
            {
                case "COUCHBASE":
                    {
                        var config = new CouchbaseConfig(_repository.Configuration);
                        gateway = new CouchbaseGateway(config, _loggerFactory.CreateLogger<CouchbaseGateway>());
                        break;
                    }
                case "SQLITE":
                    {
                        var config = new SQLiteConfig(_repository.Configuration);
                        gateway = new SQLiteGateway(config, _loggerFactory.CreateLogger<SQLiteGateway>());
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknow database gateway.");
                    }
            }

            return gateway;
        }
    }
}
