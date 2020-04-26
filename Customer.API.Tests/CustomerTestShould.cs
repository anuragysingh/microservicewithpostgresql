using Customer.API.Controllers;
using Customer.API.Core;
using Customer.API.Core.Model;
using Customer.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
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

            // event setup can be moved to the constructor
            this.user.Setup(x => x.IsValidUser(It.IsAny<string>())).Throws(new Exception("new exception"));
            this.user.Setup(x => x.IsValidUser(It.IsAny<string>())).Throws(new Exception("new exception"));
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
            var controller = this.customerController.GetData();

            Assert.NotNull(controller);
        }

        [Fact]
        public void CheckForException()
        {
            this.user.Setup(x => x.IsValidUser(It.IsAny<string>())).Throws(new Exception("new exception"));

            // if IsValidUser is called then throw an exception
            var controller = this.customerController.GetData();

            Assert.NotNull(controller);
        }

        [Fact]
        public void MultipleMockTest()
        {
            this.user.SetupSequence(x => x.IsValidUser(It.IsAny<string>()))
                .Throws(new Exception("new exception"))
                .Returns(true);

            // if IsValidUser is called then throw an exception
            var controller = this.customerController.GetData();

            Assert.NotNull(controller);
        }

        [Fact]
        public void Return200AsResponseCode()
        {
            var controller = this.customerController.GetData();
            //var result = Assert.IsType<ActionResult<User>>(controller.Result);

            this.user.VerifyAll();
            
            Assert.IsType<ActionResult<User>>(controller.Result);
        }

        [Fact]
        public void CustomMessageResponseCode()
        {
            var controller = this.customerController.GetData();
            //var result = Assert.IsType<ActionResult<User>>(controller.Result);
            this.user.Setup(x => x.IsValidUser(It.IsAny<string>())).Returns(() => false);
            var data = this.user.Object;
            this.user.Verify(x=>x.IsValidUser(It.IsAny<string>()), "IsValidIsNotNull");
            
        }

        [Fact]
        public void TimesAMetodToBeInvoked()
        {
            var controller = this.customerController.GetData();
            //var result = Assert.IsType<ActionResult<User>>(controller.Result);
            var data = this.user.Object;

            //this.user.Verify(x => x.IsValidUser(It.IsNotNull<string>()), Times.Once);
            // or
            this.user.Verify(x => x.IsValidUser(It.IsNotNull<string>()), Times.Exactly(2));

        }

        [Fact]
        public void CheckForResponseDataModel()
        {
            this.user.Setup(x => x.IsValidUser(It.IsAny<string>())).Returns(() => false);

            //when GetData() is called and if it has IsValidUser() then above return will be called
            // this return can be used to pass in fake object also
            var controller = this.customerController.GetData(); 

            var user2 = new User
            {
                Email = "123"
            };

            var result = Assert.IsType<ActionResult<User>>(controller.Result);

            Assert.Same(user2, result);
            
        }

        //[Fact]
        //public void IsValid()
        //{
        //    //this.user.Setup(x => x.IsValidUser(It.IsAny<string>("x")))
        //    //this.user.Setup(x => x.IsValidUser(It.Is<string>(number=>number.StartsWith("x"))))
        //    //this.user.Setup(x => x.IsValidUser(It.IsIn("x","y","z")))
        //    //this.user.Setup(x => x.IsValidUser(It.IsInRange("x", "y", Moq.Range.Inclusive)))
        //    this.user.Setup(x => x.IsValidUser(It.IsRegex("[b-z]", System.Text.RegularExpressions.RegexOptions.None)))
        //        .Returns(true);
        //    var boolData = "x";
        //    Assert.Equal("123", boolData);
        //}
    }
}
