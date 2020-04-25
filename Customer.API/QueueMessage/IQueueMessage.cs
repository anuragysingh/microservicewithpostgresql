using System.Threading.Tasks;

namespace Customer.API.QueueMessage
{
    public interface IQueueMessage
    {
        Task<bool> SendMessage(string message);
    }
}
