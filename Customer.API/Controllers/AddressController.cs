using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "AddressOpenAPISpecification")]
    public class AddressController : ControllerBase
    {
        // GET: api/Address

        /// <summary>
        /// To get data.
        /// </summary>
        /// <returns>string array.</returns>
        /// <remarks>
        /// This is for fetching data for **Get** type
        /// ```
        ///     Get /api/address
        ///     [
        ///         {
        ///             "get":"1234",
        ///             "data":"167"
        ///         }
        ///     ]
        /// ```
        /// </remarks>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Address/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Address
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Address/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
