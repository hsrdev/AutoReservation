using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class CustomerUpdateTest
        : TestBase
    {
        private readonly CustomerManager _target;
        public Customer Customer { get; set; }

        public CustomerUpdateTest()
        {
            _target = new CustomerManager();
        }
        
        [Fact]
        public async Task InsertCustomerTest()
        {
            // arrange
            Customer = new Customer
            {
                FirstName = "Elon",
                LastName = "Musk",
                BirthDate = new DateTime(2000, 1, 1)
            };
            // act
            await _target.Insert(Customer);
            var insertedCustomer = await _target.Get(5);
            // assert
            Assert.Equal(Customer.LastName, insertedCustomer.LastName);
        }

        [Fact]
        public async Task UpdateCustomerTest()
        {
            Customer = await _target.Get(2);
            Customer.LastName = "Ma";
            await _target.Update(Customer);
            var updatedCustomer = await _target.Get(2);
            Assert.Equal(Customer.LastName, updatedCustomer.LastName);
        }

        [Fact]
        public async Task NotExistingAccessTest()
        {
            try
            {
                // arrange & act
                Customer = await _target.Get(5);
            }
            catch (Exception e)
            {
                // assert
                Assert.Equal("Sequence contains no elements", e.Message);
            }
        }
        [Fact]
        public async Task DeleteCustomerTest()
        {
            try
            {
                // arrange
                Customer = await _target.Get(1);
                // act
                await _target.Delete(Customer);
                var deletedCustomer = await _target.Get(Customer.Id);
            }
            catch (Exception e)
            {
                // assert
                Assert.Equal("Sequence contains no elements", e.Message);
            }
        }
    }
}
