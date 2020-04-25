using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Customer.API.QueueMessage
{
    public class RabbitMQQueueMessage : IQueueMessage
    {
        public async Task<bool> SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "123",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            return true;
        }
    }
}
