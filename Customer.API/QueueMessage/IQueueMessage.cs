using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.API.QueueMessage
{
    public interface IQueueMessage
    {
        Task<bool> SendMessage(string message);
    }
}
