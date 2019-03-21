using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Net.Http;

namespace PetitionWatcher.MessageHandlers.IndexProspect.Indexer
{
    public class Index
    {        
        private readonly IConfiguration _config;

        public Index(IConfiguration config)
        {
            _config = config;
            EnsureIndex();
        }

        private void EnsureIndex()
        {            
            Console.WriteLine($"Initializing Elasticsearch. url: {_config["Elasticsearch:Url"]}");
            GetClient().CreateIndex("petitions");
        }

        public void CreateDocument(string json)
        {
            GetClient().LowLevel.Index<string>("petitions", "PetitionState", json);
        }

        private ElasticClient GetClient()
        {
            var uri = new Uri(_config["Elasticsearch:Url"]);
            return new ElasticClient(uri);
        }
    }
}
