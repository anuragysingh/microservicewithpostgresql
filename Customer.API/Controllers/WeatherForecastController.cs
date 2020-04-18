using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        public CustomerController()
        {
        }

        [HttpGet]
        public IActionResult GetData()
        {
            return Ok("ok");
        }
    }
}
