using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetitionWatcher.SchedulerApi.Publishers;
using System.Net;

namespace PetitionWatcher.SchedulerApi.Controllers
{
    public class PublishController : Controller
    {
        //private string _host = Environment.MachineName;
        //private Counter _requestCounter = Metrics.CreateCounter("PublishController_Requests", "Request count", "host", "eventType");        
        //private Counter _responseCounter = Metrics.CreateCounter("PublishController_Responses", "Response count", "host", "periodType", "status");

        private readonly ILogger _logger;

        public PublishController(ILogger<PublishController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("dataloaddue/{petitionId}")]
        public ActionResult PostEvent(int petitionId)
        {            
            _logger.LogDebug($"Publishing data load due due event.");

            var published = DataLoadDuePublisher.Publish(petitionId, _logger);
            if (!published)
            {
                //_responseCounter.Labels(_host, periodType.ToString(), "500").Inc();
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            
            //_responseCounter.Labels(_host, periodType.ToString(), "201").Inc();
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
