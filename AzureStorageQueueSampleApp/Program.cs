using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue; 

namespace QueueApp
{
    class Program
    {
        private const string ConnectionString = "";
        
        static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                string value = String.Join(" ", args);
                await SendArticleAsync(value);
                Console.WriteLine($"Sent: {value}");
            } else {
                var message = await ReceiveArticleAsync();
                Console.WriteLine($"Received {message}");
            }
        }

        static async Task SendArticleAsync(string newsMessage)
        {
            CloudQueue queue = GetQueue();
            bool createdQueue = await queue.CreateIfNotExistsAsync();
            if (createdQueue) {
                Console.WriteLine("The queue of news articles was created.");
            }
            var message = new CloudQueueMessage(newsMessage);
            await queue.AddMessageAsync(message);
        }

        static async Task<string> ReceiveArticleAsync()
        {
            CloudQueue queue = GetQueue();
            bool queueExists = await queue.ExistsAsync();
            if(queueExists)
            {
                CloudQueueMessage message = await queue.GetMessageAsync();
                if(message != null){
                    string content = message.AsString;
                    await queue.DeleteMessageAsync(message);
                    return content;
                }
            }
            return "<queue empty or not created>";
        }

        static CloudQueue GetQueue()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
            CloudQueueClient client = account.CreateCloudQueueClient();
            return client.GetQueueReference("newsqueue");
        } 
    }
}
