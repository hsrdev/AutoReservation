using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly CarManager _target;
        public Car Car { get; set; }

        public AutoUpdateTests()
        {
            _target = new CarManager();
        }

        [Fact]
        public async Task GetCarAndReservations()
        {
            // arrange & act
            Car = await _target.Get(1);
            // assert
            Assert.Equal(1, Car.Id);
            Assert.NotNull(Car.Reservations);
        }

        [Fact]
        public async Task InsertCarTest()
        {
            // arrange
            Car = new StandardCar()
            {
                Make = "Volvo V40",
                DailyRate = 100
            };
            // act
            var insertedCar = await _target.Insert(Car);
            // assert
            Assert.Equal(Car.Make, insertedCar.Make);
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            // arrange
            Car = await _target.Get(2);
            Car.DailyRate = 500;
            // act
            await _target.Update(Car);
            var updatedCar = await _target.Get(Car.Id);
            // assert
            Assert.Equal(Car.DailyRate, updatedCar.DailyRate);
        }

        [Fact]
        public async Task NotExistingAccessTest()
        {
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _target.Get(5)
            );
        }

        [Fact]
        public async Task DeleteCarTest()
        {

            // arrange
            Car = await _target.Get(1);
            // act
            await _target.Delete(Car);
            
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _target.Get(Car.Id)
            );


        }
    }
}