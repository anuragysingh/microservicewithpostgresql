using Customer.API.Core.Model;
using Customer.API.ViewModel;
using System.Collections.Generic;

namespace Customer.API.Core
{
    public interface IUser
    {
        string AddCustomer();
        List<User> GetAllUsers();
        string AddAddress();
        List<UserAddress> GetFullUserDetails(int userid);
    }
}
