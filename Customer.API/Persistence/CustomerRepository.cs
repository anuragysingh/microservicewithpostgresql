using Customer.API.Core;
using Customer.API.Core.Model;

namespace Customer.API.Persistence
{
    public class CustomerRepository : IUser
    {
        private CustomerContext _dbContext;
        public CustomerRepository(CustomerContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public string AddCustomer()
        {
            var user = new User
            {
                Email = "email@email.com",
                Id = 1234,
                Name = "Anurag"
            };
            this._dbContext.Users.Add(user);
            return "added";
        }
    }
}
