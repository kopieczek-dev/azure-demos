using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace MessageReader
{
    class Program
    {
        static QueueClient _queueClient = default!;
        static SendReceipt _sendReceipt = default!;

        static async Task Main()
        {
            // TODO Fill the connectionString 
            var connectionString = "";

            var queueName = "quickstartqueues-" + Guid.NewGuid().ToString();
            _queueClient = new QueueClient(connectionString, queueName);

            await CreateQueue();
            await AddMessagesToQueue();
            await PeekMessage();
            await UpdateMessage();
            await GetQueueLenght();
            var messages = await GetMessages();
            await DeleteMessages(messages);
            await DeleteQueue();
        }

        static async Task CreateQueue()
        {
            Console.WriteLine($"Creating queue: {_queueClient.Name}");
            await _queueClient.CreateAsync();
        }

        static async Task AddMessagesToQueue()
        {
            Console.WriteLine("\nAdding messages to the queue...");

            await _queueClient.SendMessageAsync("First message");
            await _queueClient.SendMessageAsync("Second message");

            // Save the receipt so we can update this message later
            _sendReceipt = await _queueClient.SendMessageAsync("Third message");
        }

        static async Task PeekMessage()
        {
            Console.WriteLine("\nPeek at the messages in the queue...");

            // Peek at messages in the queue
            PeekedMessage[] peekedMessages = await _queueClient.PeekMessagesAsync(maxMessages: 10);

            foreach (PeekedMessage peekedMessage in peekedMessages)
            {
                // Display the message
                Console.WriteLine($"Message: {peekedMessage.MessageText}");
            }
        }

        static async Task UpdateMessage()
        {
            Console.WriteLine("\nUpdating the third message in the queue...");

            await _queueClient.UpdateMessageAsync(
                _sendReceipt.MessageId,
                _sendReceipt.PopReceipt,
                "Third message has been updated");
        }

        static async Task GetQueueLenght()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();

            // Retrieve the cached approximate message count
            int cachedMessagesCount = properties.ApproximateMessagesCount;

            // Display number of messages
            Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");
        }

        static async Task<QueueMessage[]> GetMessages()
        {
            Console.WriteLine("\nReceiving messages from the queue...");

            // Get messages from the queue
            QueueMessage[] messages = await _queueClient.ReceiveMessagesAsync(maxMessages: 10);

            foreach (var msg in messages)
            {
                Console.WriteLine($"id: {msg.MessageId}; body: {msg.Body}");
            }

            return messages;
        }

        static async Task DeleteMessages(QueueMessage[] messages)
        {
            Console.WriteLine("\nPress Enter key to 'process' messages and delete them from the queue...");
            Console.ReadLine();

            // Process and delete messages from the queue
            foreach (QueueMessage message in messages)
            {
                // "Process" the message
                Console.WriteLine($"Deleting message: {message.MessageText}");

                // Let the service know we're finished with
                // the message and it can be safely deleted.
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }

        static async Task DeleteQueue()
        {
            Console.WriteLine("\nPress Enter key to delete the queue...");
            Console.ReadLine();

            // Clean up
            Console.WriteLine($"Deleting queue: {_queueClient.Name}");
            await _queueClient.DeleteAsync();

            Console.WriteLine("Done");
        }
    }
}