using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class ReservationServiceTests
        : ServiceTestBase
    {
        private readonly ReservationService.ReservationServiceClient _target;
        private readonly AutoService.AutoServiceClient _autoClient;
        private readonly KundeService.KundeServiceClient _kundeClient;

        public ReservationServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new ReservationService.ReservationServiceClient(Channel);
            _autoClient = new AutoService.AutoServiceClient(Channel);
            _kundeClient = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetReservationsTest()
        {
            Empty request = new Empty();

            // act
            GetAllReservationsResponse reply = _target.GetAllReservations(request);
            var list = reply.Data;
            
            // assert
            Assert.Equal(4, list.Count);
        }

        [Fact]
        public async Task GetReservationByIdTest()
        {
            // arrange
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 2 };

            // act
            ReservationDto reply = _target.GetReservation(requestId);

            // assert
            Assert.Equal("Anna", reply.Customer.FirstName);
            Assert.Equal(2, reply.ReservationNr);
        }

        [Fact]
        public async Task GetReservationByIdWithIllegalIdTest()
        {
            // arrange
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 123 };

            // act

            // assert
            Assert.Throws<RpcException>(() => _target.GetReservation(requestId));

        }

        [Fact]
        public async Task InsertReservationTest()
        {
            // arrange
            GetCarRequest carRequestId = new GetCarRequest { IdFilter = 2 };
            CarDto car = _autoClient.GetCar(carRequestId);

            GetCustomerRequest customerRequestId = new GetCustomerRequest { IdFilter = 1 };
            CustomerDto customer = _kundeClient.GetCustomer(customerRequestId);

            DateTime from = new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(2020, 12, 3, 0, 0, 0, DateTimeKind.Utc);

            ReservationDto reservation = new ReservationDto
            {
                Car = car,
                Customer = customer,
                From = from.ToTimestamp(),
                To = to.ToTimestamp(),
            };

            // act
            ReservationDto insertedReservation = _target.InsertReservation(reservation);

            // assert
            Assert.Equal(reservation.Car, insertedReservation.Car);
            Assert.Equal(customer.Id, insertedReservation.Customer.Id);
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            //arrange
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 1 };

            ReservationDto toDelete = _target.GetReservation(requestId);
            // act
            Empty emptyDelete = _target.DeleteReservation(toDelete);
            // assert
            Assert.Throws<RpcException>(() => _target.GetReservation(requestId));
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            //arrange
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 2 };
            ReservationDto toUpdate = _target.GetReservation(requestId);

            GetCustomerRequest customerRequest = new GetCustomerRequest { IdFilter = 1 };
            CustomerDto customer = _kundeClient.GetCustomer(customerRequest);


            //act
            toUpdate.Customer = customer;
            Empty emptyUpdate = _target.UpdateReservation(toUpdate);

            // assert
            ReservationDto expected = _target.GetReservation(requestId);
            Assert.Equal(1, expected.Customer.Id);
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            //arrange
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 2 };
            ReservationDto toUpdate = _target.GetReservation(requestId);

            GetCustomerRequest customer1Request = new GetCustomerRequest { IdFilter = 1 };
            CustomerDto customer1 = _kundeClient.GetCustomer(customer1Request);

            GetCustomerRequest customer2Request = new GetCustomerRequest { IdFilter = 2 };
            CustomerDto customer2 = _kundeClient.GetCustomer(customer2Request);

            //act
            toUpdate.Customer =customer1;

            Empty emptyUpdate = _target.UpdateReservation(toUpdate);
            toUpdate.Customer = customer2;
            // assert
            Assert.Throws<RpcException>(() => _target.UpdateReservation(toUpdate));
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            // arrange
            GetCarRequest carRequestId = new GetCarRequest { IdFilter = 2 };
            CarDto car = _autoClient.GetCar(carRequestId);

            GetCustomerRequest customerRequestId = new GetCustomerRequest { IdFilter = 1 };
            CustomerDto customer = _kundeClient.GetCustomer(customerRequestId);

            DateTime from = new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(2020, 12, 3, 0, 0, 0, DateTimeKind.Utc);

            ReservationDto reservation = new ReservationDto
            {
                Car = car,
                Customer = customer,
                From = to.ToTimestamp(),
                To = from.ToTimestamp(),
            };

            // act


            // assert
            Assert.Throws<RpcException>(() => _target.InsertReservation(reservation));
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            // arrange
            GetCarRequest carRequestId = new GetCarRequest { IdFilter = 2 };
            CarDto car = _autoClient.GetCar(carRequestId);

            GetCustomerRequest customerRequestId = new GetCustomerRequest { IdFilter = 1 };
            CustomerDto customer = _kundeClient.GetCustomer(customerRequestId);

            DateTime from = new DateTime(2020, 1, 19, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(2020, 1, 22, 0, 0, 0, DateTimeKind.Utc);

            ReservationDto reservation = new ReservationDto
            {
                Car = car,
                Customer = customer,
                From = from.ToTimestamp(),
                To = to.ToTimestamp(),
            };

            // act


            // assert
            Assert.Throws<RpcException>(() => _target.InsertReservation(reservation));
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 1 };
            ReservationDto toUpdate = _target.GetReservation(requestId);

            DateTime from = new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(2020, 12, 3, 0, 0, 0, DateTimeKind.Utc);

            //act
            toUpdate.From = to.ToTimestamp();
            toUpdate.To = from.ToTimestamp();

            // assert
            Assert.Throws<RpcException>(() => _target.UpdateReservation(toUpdate));
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 2 };
            ReservationDto toUpdate = _target.GetReservation(requestId);

            DateTime from = new DateTime(2020, 1, 19, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(2020, 1, 22, 0, 0, 0, DateTimeKind.Utc);

            //act
            toUpdate.From = from.ToTimestamp();
            toUpdate.To = to.ToTimestamp();

            // assert
            Assert.Throws<RpcException>(() => _target.UpdateReservation(toUpdate));
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            //arrange
            GetReservationRequest requestId = new GetReservationRequest { IdFilter = 2 };
            ReservationDto reservation = _target.GetReservation(requestId);
            //act
            CheckResponse available = _target.AvailabilityCheck(reservation);

            //assert
            Assert.True(available.IsValid);
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            //arrange
            GetCarRequest carRequestId = new GetCarRequest { IdFilter = 2 };
            CarDto car = _autoClient.GetCar(carRequestId);

            GetCustomerRequest customerRequestId = new GetCustomerRequest { IdFilter = 1 };
            CustomerDto customer = _kundeClient.GetCustomer(customerRequestId);

            DateTime from = new DateTime(2020, 1, 19, 0, 0, 0, DateTimeKind.Utc);
            DateTime to = new DateTime(2020, 1, 22, 0, 0, 0, DateTimeKind.Utc);

            ReservationDto reservation = new ReservationDto
            {
                Car = car,
                Customer = customer,
                From = from.ToTimestamp(),
                To = to.ToTimestamp(),
            };
            //act
            CheckResponse available = _target.AvailabilityCheck(reservation);

            //assert
            Assert.False(available.IsValid);
        }
    }
}