using RestaurantB.BL.Interfaces;
using RestaurantB.Models;
using MessagePack;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RestaurantB.BL.Services
{
    public class Dataflow : IDataflow
    {
        private IKafkaProducer _producer;
        

        TransformBlock<byte[], Order> entryBlock = new TransformBlock<byte[], Order>(data => MessagePackSerializer.Deserialize<Car>(data));

        Random rnd = new Random();

        public Dataflow(IKafkaProducer producer)
        {
            _producer = producer;

            var enrichBlock = new TransformBlock<Order, Order>(c =>
            {
                Console.WriteLine($"Received value: {c.Year}");
                c.Year = rnd.Next(1990, DateTime.Now.Year);

                return c;
            });

            var publishBlock = new ActionBlock<Order>(order =>
            {
                Console.WriteLine($"Updated value: {order.Year} \n");
                _producer.ProduceOrder(order);
            });

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            entryBlock.LinkTo(enrichBlock, linkOptions);
            enrichBlock.LinkTo(publishBlock, linkOptions);

        }
        public async Task SendOrder(byte[] data)
        {          
            await entryBlock.SendAsync(data);
        }  
    }
}
