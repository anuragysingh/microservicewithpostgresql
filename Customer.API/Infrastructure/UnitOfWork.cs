using Customer.API.Core;
using System.Threading.Tasks;

namespace Customer.API.Persistence
{
    public class UnitOfWork: IUnitOfWork
    {
        CustomerContext _dbContext;
        public UnitOfWork(CustomerContext context)
        {
            this._dbContext = context;
        }

        public async Task Complete()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
