using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;
using System.Collections.Generic;
using AutoReservation.Dal.Entities;
using Google.Protobuf;


namespace AutoReservation.Service.Grpc.Testing
{
    public class KundeServiceTests
        : ServiceTestBase
    {
        private readonly KundeService.KundeServiceClient _target;

        public KundeServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetCustomersTest()
        {
            // arrange
            var request = new Empty();

            // act
            GetAllCustomersResponse reply = _target.GetAllCustomers(request);
            var list = reply.Data;

            // assert
            Assert.Equal(4, list.Count);
        }

        [Fact]
        public async Task GetCustomerByIdTest()
        {
            // arrange
            var requestId = new GetCustomerRequest { IdFilter = 2 };

            // act
            CustomerDto reply = _target.GetCustomer(requestId);

            // assert
            Assert.Equal("Beil", reply.LastName);
        }

        [Fact]
        public async Task GetCustomerByIdWithIllegalIdTest()
        {
            // arrange
            var requestId = new GetCustomerRequest { IdFilter = 12 };

            // act

            // assert
            Assert.Throws<RpcException>(() => _target.GetCustomer(requestId));
        }

        [Fact]
        public async Task InsertCustomerTest()
        {
            // arrange
            DateTime date = new DateTime(2000, 1, 1, 0,0,0, DateTimeKind.Utc);
            CustomerDto cust = new CustomerDto
            {
                FirstName = "Niels",
                LastName = "Müller",
                BirthDate = date.ToTimestamp()
            };
            // act
            var requestInsert = cust;
            CustomerDto customerToInsert = _target.InsertCustomer(requestInsert);
            // assert
            var requestId = new GetCustomerRequest { IdFilter = customerToInsert.Id };
            CustomerDto insertedCustomer = _target.GetCustomer(requestId);

            Assert.Equal(customerToInsert.Id, insertedCustomer.Id);
            Assert.Equal("Müller", insertedCustomer.LastName);
        }

        [Fact]
        public async Task DeleteCustomerTest()
        {
            var requestId = new GetCustomerRequest { IdFilter = 4 };

            CustomerDto toDelete = _target.GetCustomer(requestId);
            // act
            Empty emptyDelete = _target.DeleteCustomer(toDelete);
            // assert
            Assert.Throws<RpcException>(() => _target.GetCustomer(requestId));
        }

        [Fact]
        public async Task UpdateCustomerTest()
        {
            //arrange
            var requestId = new GetCustomerRequest { IdFilter = 3 };
            CustomerDto toUpdate = _target.GetCustomer(requestId);

            //act
            toUpdate.FirstName = "Anna-Lena";
            Empty emptyUpdate = _target.UpdateCustomer(toUpdate);

            // assert
            toUpdate = _target.GetCustomer(requestId);
            Assert.Equal("Anna-Lena", toUpdate.FirstName);
        }

        [Fact]
        public async Task UpdateCustomerWithOptimisticConcurrencyTest()
        {
            //arrange
            var requestId = new GetCustomerRequest { IdFilter = 3 };
            CustomerDto toUpdate = _target.GetCustomer(requestId);

            //act
            toUpdate.FirstName = "Anna-Lena";

            Empty emptyUpdate = _target.UpdateCustomer(toUpdate);
            toUpdate.FirstName = "Anna-Lisa";
            // assert
            Assert.Throws<RpcException>(() => _target.UpdateCustomer(toUpdate));
        }
    }
}