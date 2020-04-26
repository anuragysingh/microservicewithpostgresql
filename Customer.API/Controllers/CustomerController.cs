// <copyright file="CustomerController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Customer.API.Controllers
{
    using Customer.API.Core;
    using Customer.API.Core.Model;
    using Customer.API.ViewModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController"/> class.
    /// </summary>
    [ApiController]
    [Produces("application/json", "application/xml")]
    //[Route("api/v{version:apiversion}/[controller]")] //for api versioning according to URI - not recommended approach
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    //[ApiExplorerSettings(GroupName = "CustomerOpenAPISpecification")] // for specifiying or grouping api endpoints in swagger
    //for specific controller level response type for swagger documentation
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[ApiConventionType(typeof(DefaultApiConventions))]// use this in case of mentioning default documentation for swagger
    public class CustomerController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="user">user detail.</param>
        /// <param name="unitOfWork">unit of work detail.</param>
        /// <param name="loggerfactory"></param>
        /// <param name="memoryCache"></param>
        public CustomerController(IUser user, IUnitOfWork unitOfWork, ILoggerFactory loggerfactory, IMemoryCache memoryCache)
        {
            this._user = user;
            this._unitOfWork = unitOfWork;
            this.memoryCache = memoryCache;
            this._logger = loggerfactory.CreateLogger("CustomerAPI"); // note space is not allowed
        }

        /// <summary>
        /// Get data for all users.
        /// </summary>
        /// <returns>Ok.</returns>
        [HttpGet]
        [ResponseCache(CacheProfileName = "30SecondsCacheProfile")]
        // Action level response cache overrwrites cache at Controller level
        public async Task<ActionResult<User>> GetData()
        {

            string item1 = "hello";
            string item2 = "sample";
            // adds item 1 and 2 value to serilog properties section
            this._logger.LogInformation(message: "GetData is invoked for {item1} and {item2}", item1, item2);

            //throw new Exception();
            try
            {
                var data = await this._user.GetAllUsersAsync();
                if (data.Count > 0)
                {
                    if (data[0].Email != null)
                    {
                        return this.Ok(data);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception err)
            {
                this._logger.LogError("API failure", err.Message);
            }

            return BadRequest();
        }

        [HttpGet("{cache}")]
        public ActionResult<User> GetCacheData()
        {
            //User cacheEntry;

            var userEntry = memoryCache.GetOrCreate("time", entry =>
            {
                var cacheEntry = new User
                {
                    Email = "anurag@gmail.com",
                    Name = DateTime.Now.ToString()
                };

                entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return cacheEntry;
            });

            return Ok(userEntry);
        }

        /// <summary>
        /// Get address for all users.
        /// </summary>
        /// <param name="userid">user detail.</param>
        /// <returns>Ok.</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [HttpGet("{userid}")]
        // call below api using "/api/customer/userid"
        public ActionResult<UserAddress> GetFullAdddress(int userid)
        {
            if (userid != 0)
            {
                var fullAddr = this._user.GetFullUserDetails(userid);
                if (fullAddr.Count > 0)
                {
                    return this.Ok(fullAddr);
                }
                else
                {
                    return this.NotFound();
                }
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser()
        {
            this._user.AddCustomer();
            this._user.AddAddress();
            await this._unitOfWork.Complete();
            return Ok("ok");
        }

        public bool IsValidUser(string id)
        {
            return false;
        }
    }
}
