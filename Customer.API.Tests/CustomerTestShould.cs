using Customer.API.Controllers;
using Customer.API.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using Xunit;
using Xunit.Abstractions;
namespace Customer.API.Tests
{
    [Trait("Category", "Customer")]
    public class CustomerTestShould
    {
        private readonly Mock<IUser> user;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly ILoggerFactory loggerfactory;
        private readonly Mock<IMemoryCache> memoryCache;
        private readonly CustomerController customerController;

        public CustomerTestShould()
        {
            this.user = new Mock<IUser>();
            this.unitOfWork = new Mock<IUnitOfWork>();
            this.loggerfactory = new NullLoggerFactory();
            this.memoryCache = new Mock<IMemoryCache>();
            this.customerController = new CustomerController(this.user.Object, this.unitOfWork.Object, loggerfactory, memoryCache.Object);
        }

        //Aserts against 
        // boolean = Assert.True or false
        // string = Assert.Equal, StartsWit, EndsWit, Matces (RegularExpression)
        // int = Asset.Equal, notEqual, True(for a two condition), InRange(object, lower range, higher range)
        // float = Assert.Equal(1 number, 2 number, decimal precision like 3 for 3 decimal numbers)
        // null = Assert.Null(object), NotNull
        // collections = Assert.Contains,DoesNotContain, Contains(object, w=>w.contains("word") - for atleast one word is contained
        // collections = Assert.All(object, w=> Assert.Fail(whitespacecheck(w)) - Loop through the array to check for fail
        // object = Assert.IsType<Model>(new Model) - is of strict type, isAssinableFrom<Model>(new Model) allows inheritance
        // object = Assert.NotSame(object1, object2), Same
        // exception = Assert.Throws<TypeOfException>(() => object.method(null)))

        [Fact]       
        public void GetDataShouldNotBeEmpty()
        {
            var result = this.customerController.GetData();
            Console.Write(result.Result);
            Assert.NotNull(result.Result);
        }
    }
}
