using Customer.API.QueueMessage;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.API.HealthCheckQueuePublisher
{
    public class HealthCheckQueuePublisher : IHealthCheckPublisher
    {
        private IQueueMessage _queueMessage;

        public HealthCheckQueuePublisher(IQueueMessage queueMessage)
        {
            _queueMessage = queueMessage;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            var message = JsonConvert.SerializeObject(report, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return _queueMessage.SendMessage(message);
        }
    }
}
