﻿using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace MessagePublisher
{
    public class Program
    {
        private const string serviceBusConnectionString = "";
        private const string queueName = "";

        private const int numOfMessages = 3;

        public static async Task Main(string[] args)
        {
            var client = new ServiceBusClient(serviceBusConnectionString);
            var sender = client.CreateSender(queueName);

            using (ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync())
            {
                for (int i = 1; i <= numOfMessages; i++)
                {
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                    {
                        throw new Exception($"The message {i} is too large to fit in the batch.");
                    }
                }

                try
                {
                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
                }
                finally
                {
                    await sender.DisposeAsync();
                    await client.DisposeAsync();
                }
            }
        }
    }
}