using System.Threading.Tasks;

namespace RestaurantB.BL.Interfaces
{
    public interface IDataflow
    {
        Task SendOrder(byte[] data);
    }
}
