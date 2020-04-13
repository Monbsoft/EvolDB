using Microsoft.Extensions.Configuration;
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
        private readonly Repository _repository;

        public GatewayFactory(Repository repository)
        {
            _repository = repository;
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
                        gateway = new CouchbaseGateway();
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
