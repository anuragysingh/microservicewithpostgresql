using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.API.Tests
{
    public class AddressTestData
    {
        public static IEnumerable<object[]> TestData
        {
            get
            {
                // for getting data from external data use a filereader from a csv and then map to List and return the List
                yield return new object[] { 0, 100 };
                yield return new object[] { 10, 100 };
                yield return new object[] { 10, 101 };
                yield return new object[] { 100, 100 };
            }
        }
    }
}
