using RestaurantB.BL.Interfaces;
using RestaurantB.Models;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace RestaurantB.BL.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private IProducer<int, Order> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<int, Order>(config)
                            .SetValueSerializer(new MsgPackSerializer<Order>())
                            .Build();
        }
        public async Task ProduceOrder(Order order)
        {
            var result = await _producer.ProduceAsync("Orders", new Message<int, Order>()
            {
                Key = order.Id,
                Value = order
            });
        }
    }
}
