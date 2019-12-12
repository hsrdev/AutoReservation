using System;
using System.Collections.Generic;
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
        public async Task GetCustomerWithReservation()
        {
            // arrange & act
            Customer = await _target.Get(1);
            // assert
            Assert.Equal(1, Customer.Id);
            Assert.NotNull(Customer.Reservations);
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
            var insertedCustomer = await _target.Insert(Customer);
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
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _target.Get(5)
            );
        }

        [Fact]
        public async Task DeleteCustomerTest()
        {

            // arrange
            Customer = await _target.Get(4);
            // act
            await _target.Delete(Customer);

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _target.Get(Customer.Id)
            );

            //Assert.Equal("alsdfl", ex.Message);
        }
    }
}