using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Channels;
using Voyantis.Interfaces;

namespace Voyantis.Services
{
    public class QueueService : IQueueService
    {
        private readonly ConcurrentDictionary<string, Channel<string>> _messageQueue = new ConcurrentDictionary<string, Channel<string>>();

        
        private readonly IConfiguration _configuration;
        private bool _IsRunning = false;
        private Channel<string> GetQueue(string queueName) =>
            _messageQueue.GetOrAdd(queueName, _ => Channel.CreateUnbounded<string>());
        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        public async Task<string> ConsumAsync(string queueName, int? timeout)
        {
            var channel = GetQueue(queueName);
            int timeoutMs = timeout ?? 10000; // default to 10,000 milliseconds (10 seconds)

            using var cts = new CancellationTokenSource(timeoutMs);
            try
            {
                return await channel.Reader.ReadAsync(cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                LogService.LogError($"Operaton cancel queue name: {queueName} , error message: {ex.Message}");
                throw new OperationCanceledException($"Operaton cancel queue name: {queueName} , error message: {ex.Message}");
            }
            catch (Exception ex)
            {
                LogService.LogError($"Exception when consum queue name: {queueName} , error message: {ex.Message}");
                throw new Exception($"Exception when consum queue name: {queueName} , error message: {ex.Message}");
            }
        }

        public async Task<string> ProduceAsync(string queueName, JsonElement message)
        {
            try
            {
                var channel = GetQueue(queueName);
                string messageString = message.GetRawText();
                await channel.Writer.WriteAsync(messageString);
                return messageString;
            }
            catch (Exception ex) 
            {
                LogService.LogError($"Exception when produce queue name: {queueName} Json Element : {message}, error message: {ex.Message}");
                throw new Exception($"Exception when produce queue name: {queueName} Json Element : {message}, error message: {ex.Message}");
            }
        }

    }
}
