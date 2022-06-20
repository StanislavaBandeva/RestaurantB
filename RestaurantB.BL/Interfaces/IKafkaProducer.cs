using RestaurantB.Models;
using System.Threading.Tasks;

namespace RestaurantB.BL.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceOrder(Order order);
    }
}
