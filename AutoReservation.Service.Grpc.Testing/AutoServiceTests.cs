using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class AutoServiceTests
        : ServiceTestBase
    {
        private readonly AutoService.AutoServiceClient _target;

        public AutoServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new AutoService.AutoServiceClient(Channel);
        }


        [Fact]
        public async Task GetCarsTest()
        {
            // arrange
            var request = new Empty();

            // act
            GetAllCarsResponse reply = _target.GetAllCars(request);
            var list = reply.Data;

            // assert
            Assert.Equal(4, list.Count );
            
            
        }

        [Fact]
        public async Task GetCarsByIdTest()
        {
            // arrange
            var requestId = new GetCarRequest { IdFilter = 2 };

            // act
            CarDto reply = _target.GetCar(requestId);

            // assert
            Assert.Equal("VW Golf", reply.Make);

        }

    
    
        [Fact]
        public async Task GetCarByIdWithIllegalIdTest()
        {
            // arrange
            var requestId = new GetCarRequest { IdFilter = 9 };

            // act

            // assert
            Assert.Throws<RpcException>(()=>_target.GetCar(requestId));

        }

        [Fact]
        public async Task InsertCarTest()
        {
            // arrange
            CarDto car1 = new CarDto
            {
                Make = "Volvo V40",
                DailyRate = 100,
            };
            // act
            var requestInsert = car1;
            CarDto carToInsert = _target.InsertCar(requestInsert);
            // assert
            var requestId = new GetCarRequest { IdFilter = carToInsert.Id };
            CarDto insertedCar = _target.GetCar(requestId);

            Assert.Equal(carToInsert.Id, insertedCar.Id);
            Assert.Equal("Volvo V40", insertedCar.Make);
        }

        [Fact]
        public async Task DeleteCarTest()
        {
            var requestId = new GetCarRequest { IdFilter = 4 };

            CarDto toDelete = _target.GetCar(requestId);
            // act
            Empty emptyDelete = _target.DeleteCar(toDelete);
            // assert
            Assert.Throws<RpcException>(() => _target.GetCar(requestId));
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            //arrange
            var requestId = new GetCarRequest { IdFilter = 3 };
            CarDto toUpdate = _target.GetCar(requestId);

            //act
            toUpdate.Make = "Subaru";
            Empty emptyUpdate = _target.UpdateCar(toUpdate);

            // assert
            toUpdate = _target.GetCar(requestId);
            Assert.Equal("Subaru", toUpdate.Make);
        }

        [Fact]
        public async Task UpdateCarWithOptimisticConcurrencyTest()
        {
            //arrange
            var requestId = new GetCarRequest { IdFilter = 3 };
            CarDto toUpdate = _target.GetCar(requestId);
            CarDto secondToUpdate = _target.GetCar(requestId);

            //act
            toUpdate.Make = "Subaru";

            Empty emptyUpdate = _target.UpdateCar(toUpdate);
            toUpdate.Make = "BMW";
            // assert
            Assert.Throws<RpcException>(() => _target.UpdateCar(toUpdate));

        }
    }
}