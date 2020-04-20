using Customer.API.Core;
using Customer.API.Core.Model;
using Customer.API.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                UserId = 1234,
                Name = "Anurag"
            };
            this._dbContext.Users.Add(user);
            return "added";
        }

        public string AddAddress()
        {
            var address = new Address
            {
                AddressID = 2234,
                UserId = 1234,
                Address1 = 1209,
                Address2 = 187
            };
            this._dbContext.Address.Add(address);
            return "added";
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return this._dbContext.Users.OrderBy(name=>name.Name).ToList();
        }

        public List<UserAddress> GetFullUserDetails(int userid)
        {
            var address = from adrs in this._dbContext.Address
                          join usr in this._dbContext.Users
                          on adrs.UserId equals usr.UserId
                          where usr.UserId == userid
                          select new
                          {
                              usr.Name,
                              usr.Email,
                              adrs.Address1,
                              adrs.Address2
                          };
            List<UserAddress> usrAdr = new List<UserAddress>();

            foreach(var userDet in address)
            {
                usrAdr.Add(new UserAddress
                {
                    UserName = userDet.Name,
                    Email = userDet.Email,
                    Address1 = userDet.Address1.ToString(),
                    Address2 = userDet.Address2.ToString()
                });
            }
            return usrAdr;
        }
    }
}
