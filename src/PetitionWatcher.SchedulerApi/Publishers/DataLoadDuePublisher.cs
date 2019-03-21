using Microsoft.Extensions.Logging;
using PetitionWatcher.Messaging;
using PetitionWatcher.Messaging.Messages.Events;
using System;

namespace PetitionWatcher.SchedulerApi.Publishers
{
    public static class DataLoadDuePublisher
    {
        //private static string _Host = Environment.MachineName;
        //private static Counter _EventCounter = Metrics.CreateCounter("DeviceUsageSummaryDuePublisher_Events", "Event count", "host", "periodType", "status");

        public static bool Publish(int petitionId)
        {
            return Publish(petitionId, null);
        }

        public static bool Publish(int petitionId, ILogger logger)
        {            
            var eventMessage = new DataLoadDueEvent
            {
                PetitionId = petitionId,
                DueAtUtc = DateTime.UtcNow
            };
            try
            {
                MessageQueue.Publish(eventMessage);
                logger?.LogDebug($"Published data load due event for petition id: {petitionId}");
                //_EventCounter.Labels(_Host, periodType.ToString(), "published").Inc();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"Publish data load due event FAILED for petition id: {petitionId}, message: {ex.Message}");
                return false;
            }

            return true;
        }
    }
}