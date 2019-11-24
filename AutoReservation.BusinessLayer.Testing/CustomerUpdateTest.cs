using System;
using System.Threading.Tasks;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class CustomerUpdateTest
        : TestBase
    {
        private readonly KundeManager _target;

        public CustomerUpdateTest()
        {
            _target = new KundeManager();
        }
        
        [Fact]
        public async Task UpdateCustomerTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}
