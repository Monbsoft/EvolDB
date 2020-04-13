using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Couchbase;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
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
                        gateway = new CouchbaseGateway(_loggerFactory.CreateLogger<CouchbaseGateway>());
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
