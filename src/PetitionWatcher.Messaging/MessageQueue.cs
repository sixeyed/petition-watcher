﻿using NATS.Client;
using PetitionWatcher.Core;
using PetitionWatcher.Messaging.Messages;

namespace PetitionWatcher.Messaging
{
    public static class MessageQueue
    {

        public static void Publish<TMessage>(TMessage message)
            where TMessage : Message
        {
            using (var connection = CreateConnection())
            {
                var data = MessageHelper.ToData(message);
                connection.Publish(message.Subject, data);
            }
        }

        public static IConnection CreateConnection()
        {
            return new ConnectionFactory().CreateConnection(Config.Current["MessageQueue:Url"]);
        }
    }
}
