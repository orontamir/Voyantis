using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Channels;

namespace Voyantis.Interfaces
{
    public interface IQueueService
    {
        public Task<string> ProduceAsync(string queueName, JsonElement message);

        public Task<string> ConsumAsync(string queueName, int? timeout);
    }
}
