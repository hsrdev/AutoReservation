using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly AutoManager _target;
        public Car Car { get; set; }

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }

        [Fact]
        public async Task InsertCarTest()
        {
            //arrange
            Car = new StandardCar()
            {
                Make = "Volvo V40",
                DailyRate = 100
            };
            //act
            await _target.Insert(Car);
            var insertedCar = await _target.Get(Car.Id);
            //assert
            Assert.Equal(Car.Id, insertedCar.Id);
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            //arrange
            Car = await _target.Get(2);
            Car.DailyRate = 500;
            //act
            await _target.Update(Car);
            var updatedCar = await _target.Get(Car.Id);
            //assert
            Assert.Equal(Car.DailyRate, updatedCar.DailyRate);
        }
    }
}
