using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Customer.API.Tests
{
    [Trait("Category", "Address")]
    public class AddressShould
    {

        [Fact]        
        public void GetAddress()
        {
            Assert.Equal("address", "address");
        }

        [Fact(Skip = "Not required to run")]
        public void GetAddress2()
        {
            Assert.Equal("address", "address");
        }

        // Data driven test where multiple input values needs to be tested
        [Theory] // to specify its data driven test
        [InlineData(100,100)]
        [InlineData(10, 100)]
        [InlineData(101, 100)]
        public void GetAddress3(int first, int second)
        {
            Assert.Equal(first, second);
        }

        // Data driven test to be used across classes
        [Theory] // to specify its data driven test

        // specify static method name along with class type 
        [MemberData(nameof(AddressTestData.TestData), MemberType = typeof(AddressTestData))]
        public void GetAddress4(int first, int second)
        {
            Assert.Equal(first, second);
        }


        // Custom Data driven test to be used across classes
        [Theory] // to specify its data driven test

        // specify static method name along with class type 
        [CustomTestData]
        public void GetAddress5(int first, int second)
        {
            Assert.Equal(first, second);
        }
    }
}
