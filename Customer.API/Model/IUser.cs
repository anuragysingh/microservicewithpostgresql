using Customer.API.Core.Model;
using Customer.API.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.API.Core
{
    public interface IUser
    {
        string AddCustomer();
        Task<List<User>> GetAllUsersAsync();
        string AddAddress();
        List<UserAddress> GetFullUserDetails(int userid);
        bool IsValidUser(string userid);
    }
}
