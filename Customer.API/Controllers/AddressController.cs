﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Customer.API.Controllers
{
    [ApiController]
    [Produces("application/json", "application/xml")]
    //[Route("api/v{version:apiversion}/[controller]")] //for api versioning according to URI - not recommended approach
    [Route("api/[controller]")]
    [ApiVersion("2.0")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    //[ApiExplorerSettings(GroupName = "AddressOpenAPISpecification")]
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

        public bool IsValid()
        {
            return false;
        }
    }
}
