using Customer.API.Core;
using Customer.API.Core.Model;
using Customer.API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Customer.API.Persistence
{
    public class CustomerRepository : IUser, IDisposable
    {
        private CustomerContext _dbContext;
        private readonly IHttpClientFactory httpClientFactory;

        public CustomerRepository(CustomerContext dbContext,
            IHttpClientFactory httpClientFactory)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
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
            // using httpclientfactory:
            var httpClient = httpClientFactory.CreateClient();

            var response = httpClient.GetAsync("url");
            if (response.IsCompletedSuccessfully)
            {
                // return ok or action
            }

            return await this._dbContext.Users.OrderBy(name => name.Name).ToListAsync();
        }

        public List<UserAddressDTO> GetFullUserDetails(int userid)
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
            List<UserAddressDTO> usrAdr = new List<UserAddressDTO>();

            foreach (var userDet in address)
            {
                usrAdr.Add(new UserAddressDTO
                {
                    UserName = userDet.Name,
                    Email = userDet.Email,
                    Address1 = userDet.Address1.ToString(),
                    Address2 = userDet.Address2.ToString()
                });
            }

            // for testing purpose
            List<UserAddressDTO> usrAdrTest = new List<UserAddressDTO>();
                usrAdr.Add(new UserAddressDTO
                {
                    UserName = "123"
                });

            if (this.IsValidUser(usrAdr[0].UserName))
                return usrAdrTest;
            else
                return usrAdr;
        }

        public bool IsValidUser(string userid)
        {
            return false;
        }

        public void Dispose()
        {
            if (this._dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
