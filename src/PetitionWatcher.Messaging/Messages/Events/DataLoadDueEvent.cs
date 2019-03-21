using System;

namespace PetitionWatcher.Messaging.Messages.Events
{
    public class DataLoadDueEvent : Message
    {
        public int PetitionId { get; set; }

        public DateTime DueAtUtc { get; set; }        

        public static string MessageSubject = "events.prospect.signedup";

        public override string Subject { get { return MessageSubject; } }
    }
}
