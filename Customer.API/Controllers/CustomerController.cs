using Customer.API.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUser user, IUnitOfWork unitOfWork)
        {
            this._user = user;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            return Ok("ok");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser()
        {
            this._user.AddCustomer();
            await this._unitOfWork.Complete();
            return Ok("ok");
        }
    }
}
