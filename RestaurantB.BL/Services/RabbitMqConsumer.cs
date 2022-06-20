using RestaurantB.BL.Interfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Restaurant;
using RabbitMQ.Restaurant.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantB.BL.Services
{
    public class RabbitMqConsumer : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private IDataflow _dataflow;

        public RabbitMqConsumer(IDataflow dataflow)
        {

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("order_queue", true, false);

            _dataflow = dataflow;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                await _dataflow.SendOrder(ea.Body.ToArray());
            };

            _channel.BasicConsume("order_queue", autoAck: true, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
    }
}
