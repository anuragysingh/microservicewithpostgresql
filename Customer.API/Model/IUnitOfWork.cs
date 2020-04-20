using System.Threading.Tasks;

namespace Customer.API.Core
{
    public interface IUnitOfWork
    {
        Task Complete();
    }
}
