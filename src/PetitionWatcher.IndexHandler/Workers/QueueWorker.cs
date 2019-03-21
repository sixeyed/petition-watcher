using Microsoft.Extensions.Configuration;
using NATS.Client;
using PetitionWatcher.Core;
using PetitionWatcher.MessageHandlers.IndexProspect.Indexer;
using PetitionWatcher.Messaging;
using PetitionWatcher.Messaging.Messages.Events;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace PetitionWatcher.MessageHandlers.IndexProspect.Workers
{
    public class QueueWorker
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private static HttpClient _HttpClient = new HttpClient();
        private const string QUEUE_GROUP = "index-handler";

        //private static Counter _EventCounter = Metrics.CreateCounter("IndexHandler_Events", "Event count", "host", "status");
        //private static string _Host = Environment.MachineName;

        private readonly IConfiguration _config;
        private readonly Index _index;

        public QueueWorker(IConfiguration config, Index index)
        {
            _config = config;
            _index = index;

            _HttpClient.BaseAddress = new Uri("https://petition.parliament.uk/petitions/");
            _HttpClient.DefaultRequestHeaders.Accept.Clear();
            _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _HttpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public void Start()
        {
            Console.WriteLine($"Connecting to message queue url: {Config.Current["MessageQueue:Url"]}");
            using (var connection = MessageQueue.CreateConnection())
            {
                var subscription = connection.SubscribeAsync(DataLoadDueEvent.MessageSubject, QUEUE_GROUP);
                subscription.MessageHandler += IndexPetitionState;
                subscription.Start();
                Console.WriteLine($"Listening on subject: {DataLoadDueEvent.MessageSubject}, queue: {QUEUE_GROUP}");

                _ResetEvent.WaitOne();
                connection.Close();
            }
        }

        private void IndexPetitionState(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine($"Received message, subject: {e.Message.Subject}");
            var eventMessage = MessageHelper.FromData<DataLoadDueEvent>(e.Message.Data);
            Console.WriteLine($"Indexing petition id: {eventMessage.PetitionId}, load due at: {eventMessage.DueAtUtc}; event ID: {eventMessage.CorrelationId}");

            IndexPetitionState(eventMessage.PetitionId);
        }

        public void IndexPetitionState(int petitionId)
        {
            try
            {
                var json = _HttpClient.GetStringAsync($"{petitionId}.json").Result;
                Console.WriteLine($"Petition data retrieved; petition ID: {petitionId}");
                _index.CreateDocument(json);
                Console.WriteLine($"Petition data indexed; petition ID: {petitionId}");                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Petition index FAILED; petition ID: {petitionId}, ex: {ex}");                
            }
        }
    }
}
