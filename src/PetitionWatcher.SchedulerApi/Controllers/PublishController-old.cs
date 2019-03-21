using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;
using Hudl.Messaging;
using Hudl.Messaging.Messages;
using System.Net;
using Hudl.EventPublisherApi.Publishers;
using Hudl.Entities;

namespace Hudl.EventPublisherApi.Controllers
{
    public class PublishController : Controller
    {
        private string _host = Environment.MachineName;
        private Counter _requestCounter = Metrics.CreateCounter("PublishController_Requests", "Request count", "host", "eventType");
        
        private Counter _responseCounter = Metrics.CreateCounter("PublishController_Responses", "Response count", "host", "periodType", "status");

        private readonly ILogger _logger;

        public PublishController(ILogger<PublishController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("usagesummarydue/{periodType}")]
        public ActionResult PostEvent(SummaryPeriod periodType)
        {
            _requestCounter.Labels(_host, periodType.ToString()).Inc();                     
            _logger.LogDebug($"Publishing device usage summary due event.");

            var published = DeviceUsageSummaryDuePublisher.Publish(periodType);
            if (!published)
            {
                _responseCounter.Labels(_host, periodType.ToString(), "500").Inc();
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            
            _responseCounter.Labels(_host, periodType.ToString(), "201").Inc();
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
